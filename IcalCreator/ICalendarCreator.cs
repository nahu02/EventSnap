using System.Text.Json.Nodes;
using Ical.Net;

namespace IcalCreator
{
    public interface ICalendarCreator
    {
        /// <summary>
        ///     Create an Ical.Net.Calendar from a JSON object.
        ///     The Calendar will contain a single event.
        /// </summary>
        /// <param name="jsonObjectOfEvent">JSON object describing a calendar event</param>
        /// <returns>Parsed Ical.Net.Calendar with a single event.</returns>
        Calendar CreateIcalFromJson(JsonObject jsonObjectOfEvent);

        /// <summary>
        ///     Create an Ical.Net.Calendar from a CalendarEventProperties object.
        ///     The Calendar will contain a single event.
        /// </summary>
        /// <param name="eventProperties">Object describing a calendar event</param>
        /// <returns>Parsed Ical.Net.Calendar with a single event.</returns>
        Calendar CreateIcal(CalendarEventProperties eventProperties);
    }
}