using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace AiCommunicator
{
    public interface ICalendarEventInterpreter
    {
        public ILogger? Logger { get; set; }

        public Task<JsonObject> EventToJsonAsync(string eventText);

        public Task<IcalCreator.CalendarEventProperties> EventToIcalCreatorEventPropertiesAsync(string eventText);
    }
}