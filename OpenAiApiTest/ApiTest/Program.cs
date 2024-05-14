using AiCommunicator;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace ApiTest
{
    internal class Program
    {
        private static ILoggerFactory MainLoggerFactory => LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        private static async Task Main(string[] args)
        {
            await OpenaiCalendarEventInterpreterTest();
        }

        private static async Task OpenaiCalendarEventInterpreterTest()
        {      
            // read api key from ~/.openai
            var key = APIAuthentication.LoadFromPath()?.ApiKey;

            ICalendarEventInterpreter communicator = new OpenaiCalendarEventInterpreter(key!);
            communicator.Logger = MainLoggerFactory.CreateLogger<OpenaiCalendarEventInterpreter>();

            var json = await communicator.EventToJson(
                "Dear students, your exam will take place tomorrow at IB027, from 9am to 10:15am. You may use a calculator. Good luck!");

            Console.WriteLine(json.ToString());
        }

        private static async Task NugetOpenAiApiTest()
        {
            var api = new OpenAIAPI(); // uses default, env, or config file

            api.HttpClientFactory = new LoggingHttpClientFactory(); // log all requests and responses

            var chat = api.Chat.CreateConversation();
            chat.RequestParameters.ResponseFormat = ChatRequest.ResponseFormats.JsonObject;

            chat.Model = Model.GPT4_Turbo;

            chat.RequestParameters.Temperature = 0;

            chat.AppendSystemMessage("""
                                     You are an assistant helping the user create calender events.
                                     The user will tell you about an event they want to add to their calender and you create the event for them.
                                     The events must be in a JSON format, and may include a Summary, a Start, an End, a Location and a Description.
                                     If the JSON includes a Start, it must also include an End.
                                     The JSON can be incomplete but must be a valid JSON object with at least one key-value pair.
                                     You only ever respond with valid JSONs. You do not say anything else.
                                     """);

            chat.AppendUserInput("Tomorrow evening I have a meeting with John at 5pm at the office");
            var tomorrow5Pm = DateTime.Now.AddDays(1).Date.AddHours(17);
            var tomorrow6Pm = DateTime.Now.AddDays(1).Date.AddHours(18);
            chat.AppendExampleChatbotOutput($$"""
                                              {
                                                "Summary": "Meeting with John",
                                                "Start": "{{tomorrow5Pm}}",
                                                "End": "{{tomorrow6Pm}}",
                                                "Location": "Office"
                                              }
                                              """);
            chat.AppendUserInput(
                "Hey Joe can you take care of the kids saturday morning? Make sure they eat breakfast and do their homework.");
            var saturday9Am = DateTime.Now.AddDays(4).Date.AddHours(9);
            var saturday12Pm = DateTime.Now.AddDays(4).Date.AddHours(12);
            chat.AppendExampleChatbotOutput($$"""
                                              {
                                                "Summary": "Take care of the kids",
                                                "Start": "{{saturday9Am}}",
                                                "End": "{{saturday12Pm}}"
                                                "Description": "Make sure they eat breakfast and do their homework."
                                              }
                                              """);

            chat.AppendUserInput(
                "Dear students, your exam will take place tomorrow at IB027, from 9am to 10:15am. You may use a calculator. Good luck!");
            var response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);

            while (true)
            {
                Console.Write("You: ");
                var user_input = Console.ReadLine();
                chat.AppendUserInput(user_input);
                response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine("Chatbot: " + response);
            }
        }
    }
}