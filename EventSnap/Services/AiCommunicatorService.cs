using AiCommunicator;
using EventSnap.Models;

namespace EventSnap.Services
{
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

        public AiCommunicatorService(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<EventModel> GetEventFromNaturalLanguageTextAsync(string naturalLanguageText)
        {
            ArgumentNullException.ThrowIfNull(naturalLanguageText);

            var task = _calendarEventInterpreter.EventToIcalCreatorEventPropertiesAsync(naturalLanguageText);

            var eventProperties = await task;
            var model = EventModelFromEventProperties(eventProperties);
            return model;

        }

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
