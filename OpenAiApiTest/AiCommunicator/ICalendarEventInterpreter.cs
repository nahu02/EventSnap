using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace AiCommunicator
{
    /// <summary>
    /// Represents an interface for turning natural language event text into calendar event objects.
    /// </summary>
    public interface ICalendarEventInterpreter
    {
        /// <summary>
        /// Gets or sets the logger used for logging events.
        /// </summary>
        public ILogger? Logger { get; set; }

        /// <summary>
        /// Converts the specified event text to a <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="eventText">The event text to convert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the converted <see cref="JsonObject"/>.</returns>
        public Task<JsonObject> EventToJsonAsync(string eventText);

        /// <summary>
        /// Converts the specified event text to an instance of <see cref="IcalCreator.CalendarEventProperties"/>.
        /// </summary>
        /// <param name="eventText">The event text to convert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the converted <see cref="IcalCreator.CalendarEventProperties"/>.</returns>
        public Task<IcalCreator.CalendarEventProperties> EventToIcalCreatorEventPropertiesAsync(string eventText);
    }
}