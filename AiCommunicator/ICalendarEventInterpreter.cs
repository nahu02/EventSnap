using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace AiCommunicator
{
    public interface ICalendarEventInterpreter
    {
        public ILogger? Logger { get; set; }
        
        Task<JsonObject> EventToJson(string eventText);
    }
}