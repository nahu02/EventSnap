using OpenAI_API;
using OpenAI_API.Models;

namespace ApiTest;

class Program
{
    static async Task Main(string[] args)
    {
        OpenAIAPI api = new OpenAIAPI(); // uses default, env, or config file
        Console.WriteLine("Hello, World!");
        
        var chat = api.Chat.CreateConversation();

        // chat.Model = Model.ChatGPTTurbo;  // GPT 3.5 Turbo
        chat.Model = Model.GPT4_Turbo;
        
        chat.RequestParameters.Temperature = 0;

        chat.AppendSystemMessage("You are a teacher who helps children understand if things are animals or not.  If the user tells you an animal, you say \"yes\".  If the user tells you something that is not an animal, you say \"no\".  You only ever respond with \"yes\" or \"no\".  You do not say anything else.");

// give a few examples as user and assistant
        chat.AppendUserInput("Is this an animal? Cat");
        chat.AppendExampleChatbotOutput("Yes");
        chat.AppendUserInput("Is this an animal? House");
        chat.AppendExampleChatbotOutput("No");

// now let's ask it a question
        chat.AppendUserInput("Is this an animal? Dog");
// and get the response
        string response = await chat.GetResponseFromChatbotAsync();
        Console.WriteLine(response); // "Yes"
    }
}
