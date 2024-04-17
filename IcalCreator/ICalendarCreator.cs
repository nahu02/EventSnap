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
        Calendar CreateIcalFromJson(JsonObject jsonObjectOfEvent);
    }
}