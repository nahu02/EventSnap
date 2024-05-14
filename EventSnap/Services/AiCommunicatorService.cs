using AiCommunicator;
using EventSnap.Models;

namespace EventSnap.Services
{
    /// <summary>
    /// Represents a service that communicates with an AI provider to interpret natural language text and extract event information.
    /// </summary>
    public class AiCommunicatorService
    {
        private readonly SettingsService _settingsService;
        private ICalendarEventInterpreter _calendarEventInterpreter
        {
            get
            {
                // If more AI providers are added, we could create a different EventInterpreter based on a corresponding setting
                return new OpenaiCalendarEventInterpreter(_settingsService.OpenAiApiKey) { ModelName = _settingsService.OpenAiModel };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AiCommunicatorService"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        public AiCommunicatorService(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Extract event information from natural language text.
        /// </summary>
        /// <param name="naturalLanguageText">The natural language text representing the event.</param>
        /// <returns>An asynchronous task that represents the operation. The task result contains the event model.</returns>
        public async Task<EventModel> GetEventFromNaturalLanguageTextAsync(string naturalLanguageText)
        {
            ArgumentNullException.ThrowIfNull(naturalLanguageText);

            var task = _calendarEventInterpreter.EventToIcalCreatorEventPropertiesAsync(naturalLanguageText);

            var eventProperties = await task;
            var model = EventModelFromEventProperties(eventProperties);
            return model;

        }

        /// <summary>
        /// Converts an <see cref="IcalCreator.CalendarEventProperties"/> object to an <see cref="EventModel"/> object.
        /// </summary>
        private EventModel EventModelFromEventProperties(IcalCreator.CalendarEventProperties eventProperties)
        {
            ArgumentNullException.ThrowIfNull(eventProperties);

            var model = new EventModel
            {
                Title = eventProperties.Summary,
                Description = eventProperties.Description,
                Location = eventProperties.Location
            };

            if (eventProperties.Start != null)
            {
                model.StartDateTime = DateTime.Parse(eventProperties.Start);
            }

            if (eventProperties.End != null)
            {
                model.EndDateTime = DateTime.Parse(eventProperties.End);
            }

            return model;
        }
    }
}
