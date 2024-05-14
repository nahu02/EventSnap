using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace AiCommunicator
{
    public class OpenaiCalendarEventInterpreter : ICalendarEventInterpreter
    {
        public OpenaiCalendarEventInterpreter(string apiKey)
        {
            ApiKey = apiKey;
            RequestUrl = "https://api.openai.com/v1/chat/completions";
            ModelName = "gpt-4-turbo-preview";
        }


        public string ApiKey { private get; set; }

        public string RequestUrl { get; set; }

        public string ModelName { get; set; }

        public ILogger? Logger { get; set; }

        public async Task<JsonObject> EventToJsonAsync(string eventText)
        {
            var postBody = CreatePostBody(eventText);
            using var client = new HttpClient();

            using var request = new HttpRequestMessage(HttpMethod.Post, RequestUrl);
            using var requestContent = new StringContent(postBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {ApiKey}");
            request.Content = requestContent;

            Logger?.Log(LogLevel.Debug, $"Sending request to OpenAI API:\n{request}\n{postBody}");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Logger?.Log(LogLevel.Error,
                    $"Failed to get response from OpenAI API to request:\n{request}\n{postBody}");
                throw new Exception($"Failed to get response from OpenAI API. Status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Logger?.Log(LogLevel.Debug, $"Received response from OpenAI API:\n{responseContent}");

            string chatbotReply;

            try
            {
                var responseJson = JsonNode.Parse(responseContent);
                var choice = responseJson["choices"].AsArray()[0];
                chatbotReply = choice["message"]["content"].ToString();
            }
            catch (Exception e)
            {
                Logger?.Log(LogLevel.Error, $"Failed to parse response from OpenAI API. Response:\n{responseContent}");
                throw new Exception("Failed to parse response from OpenAI API.", e);
            }

            try
            {
                var eventJson = JsonNode.Parse(chatbotReply);
                return eventJson.AsObject();
            }
            catch (JsonException e)
            {
                Logger?.Log(LogLevel.Error, $"Failed to parse JSON from OpenAI API reply. Reply: {chatbotReply}");
                throw new Exception("Failed to parse JSON from OpenAI API reply.", e);
            }
        }

        public async Task<IcalCreator.CalendarEventProperties> EventToIcalCreatorEventPropertiesAsync(string eventText)
        {
            var eventJson = await EventToJsonAsync(eventText).ConfigureAwait(false);
            var properties = new IcalCreator.CalendarEventProperties();

            var propertySetters = new Dictionary<string, Action<string>>
            {
                { "Summary", value => properties.Summary = value },
                { "Location", value => properties.Location = value },
                { "Description", value => properties.Description = value },
                { "Start", value => properties.Start = value },
                { "End", value => properties.End = value },
            };

            foreach (var key in propertySetters.Keys)
            {
                if (eventJson.ContainsKey(key))
                {
                    propertySetters[key](eventJson[key].ToString());
                }
            }

            return properties;
        }

        private string CreatePostBody(string eventText)
        {
            var systemContext = """
                                You are an assistant helping the user create calender events.
                                The user will tell you about an event they want to add to their calender and you create the event for them.
                                The events must be in a JSON format, and may include a Summary, a Start, an End, a Location and a Description.
                                If the JSON includes a Start, it must also include an End.
                                The JSON can be incomplete but must be a valid JSON object with at least one key-value pair.
                                You only ever respond with valid JSONs. You do not say anything else.
                                """;

            var tomorrow5Pm = DateTime.Now.AddDays(1).Date.AddHours(17);
            var tomorrow6Pm = DateTime.Now.AddDays(1).Date.AddHours(18);

            var example1Prompt = "Tomorrow evening I have a meeting with John at 5pm at the office";
            var example1Reply = $$"""
                                  {
                                    "Summary": "Meeting with John",
                                    "Start": "{{tomorrow5Pm}}",
                                    "End": "{{tomorrow6Pm}}",
                                    "Location": "Office"
                                  }
                                  """.Replace("\"", "\\\"");

            var daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)DateTime.Now.DayOfWeek + 7) % 7;
            var saturday9Am = DateTime.Now.AddDays(daysUntilSaturday).Date.AddHours(9);
            var saturday12Pm = DateTime.Now.AddDays(daysUntilSaturday).Date.AddHours(12);

            var example2Prompt =
                "Hey Joe can you take care of the kids saturday morning? Make sure they eat breakfast and do their homework.";
            var example2Reply = $$"""
                                  {
                                    "Summary": "Take care of the kids",
                                    "Start": "{{saturday9Am}}",
                                    "End": "{{saturday12Pm}}"
                                    "Description": "Make sure they eat breakfast and do their homework."
                                  }
                                  """.Replace("\"", "\\\"");

            var ret = $$"""
                        {
                          "model": "{{ModelName}}",
                          "messages": [
                            {
                              "role": "system",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{systemContext}}"
                                }
                              ]
                            },
                            {
                              "role": "user",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{example1Prompt}}"
                                }
                              ]
                            },
                            {
                              "role": "assistant",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{example1Reply}}"
                                }
                              ]
                            },
                            {
                              "role": "user",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{example2Prompt}}"
                                }
                              ]
                            },
                            {
                              "role": "assistant",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{example2Reply}}"
                                }
                              ]
                            },
                            {
                              "role": "user",
                              "content": [
                                {
                                  "type": "text",
                                  "text": "{{eventText}}"
                                }
                              ]
                            }
                          ],
                          "temperature": 0,
                          "n": 1,
                          "stream": false
                        }
                        """;
            ret = Regex.Replace(ret, @"\s+", " ");
            return ret;
        }
    }
}