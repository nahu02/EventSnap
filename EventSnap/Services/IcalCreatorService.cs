using EventSnap.Models;
using Ical.Net;
using Ical.Net.Serialization;
using IcalCreator;


namespace EventSnap.Services
{
    public class IcalCreatorService
    {
        public async Task AddEventToCalendarAsync(EventModel eventModel)
        {
            ArgumentNullException.ThrowIfNull(eventModel);

            System.Diagnostics.Debug.WriteLine($"AddEventToCalendarAsync;\n" +
                $"eventModel.Title : {eventModel.Title},\n" +
                $"eventModel.Description : {eventModel.Description},\n" +
                $"eventModel.Location : {eventModel.Location},\n" +
                $"eventModel.StartDateTime : {eventModel.StartDateTime},\n" +
                $"eventModel.EndDateTime : {eventModel.EndDateTime}");

            var calendar = createIcalFromEventModel(eventModel);
            var icsFile = await createIcsFileFromCalendarAsync(calendar, "event.ics");
            await openIcsFile(icsFile, "Read ical file").ConfigureAwait(false);
        }

        private async Task openIcsFile(string file, string openRequestTitle = "")
        {
            Launcher.Default.OpenAsync(new OpenFileRequest(openRequestTitle, new ReadOnlyFile(file)));
        }

        private async Task<string> createIcsFileFromCalendarAsync(Calendar calendar, string fileName)
        {
            var serializer = new CalendarSerializer();
            string icalContent = await Task.Run(() => serializer.SerializeToString(calendar)).ConfigureAwait(false);

            string file = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(file, icalContent).ConfigureAwait(false);

            return file;
        }

        private Calendar createIcalFromEventModel(EventModel eventModel)
        {
            ArgumentNullException.ThrowIfNull(eventModel);

            ICalendarCreator calendarCreator = new CalendarCreator();

            var eventProperties = new CalendarEventProperties
            {
                Summary = eventModel.Title,
                Description = eventModel.Description,
                Location = eventModel.Location,
                Start = eventModel.StartDateTime.ToString(),
                End = eventModel.EndDateTime.ToString()
            };

            return calendarCreator.CreateIcal(eventProperties);
        }
    }
}
