using System.Text.Json.Nodes;

namespace AiCommunicator
{
    public interface ICalendarEventInterpreter
    {
        Task<JsonObject> EventToJson(string eventText);
    }
}