using EventSnap.Models;
using Ical.Net;
using Ical.Net.Serialization;
using IcalCreator;


namespace EventSnap.Services
{
    /// <summary>
    /// Represents a service that can create an iCalendar (ICS) file from an event model.
    /// </summary>
    public class IcalCreatorService
    {
        /// <summary>
        /// Creates a new ICS calendar from the specified event model, and then opens the ICS file.
        /// (Depending on the platform, this usually opens the calendar app, where the user can add the event to their own calendar.)
        /// </summary>
        /// <param name="eventModel">The event model to create the ICS file from.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Opens the specified ICS file asynchronously.
        /// </summary>
        /// <param name="file">The path of the ICS file to open.</param>
        /// <param name="openRequestTitle">The title of the open request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task openIcsFile(string file, string openRequestTitle = "")
        {
            await Launcher.Default.OpenAsync(new OpenFileRequest(openRequestTitle, new ReadOnlyFile(file))).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates an ICS file from the specified calendar asynchronously.
        /// </summary>
        /// <param name="calendar">The calendar to create the ICS file from.</param>
        /// <param name="fileName">The name of the ICS file.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the path of the created ICS file.</returns>
        private async Task<string> createIcsFileFromCalendarAsync(Calendar calendar, string fileName)
        {
            var serializer = new CalendarSerializer();
            string icalContent = await Task.Run(() => serializer.SerializeToString(calendar)).ConfigureAwait(false);

            string file = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(file, icalContent).ConfigureAwait(false);

            return file;
        }

        /// <summary>
        /// Creates an iCalendar (ICS) object from the specified event model.
        /// </summary>
        /// <param name="eventModel">The event model to create the iCalendar from.</param>
        /// <returns>The created iCalendar object.</returns>
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
