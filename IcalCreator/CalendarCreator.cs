using System.Text.Json.Nodes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace IcalCreator
{
    public class CalendarCreator : ICalendarCreator
    {
        /// <summary>
        ///     Create an Ical.Net.Calendar from a JSON object.
        ///     The Calendar will contain a single event.
        ///     <para>
        ///         The following properties are supported:
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>Summary</description>
        ///             </item>
        ///             <item>
        ///                 <description>Description</description>
        ///             </item>
        ///             <item>
        ///                 <description>Location</description>
        ///             </item>
        ///             <item>
        ///                 <description>Start</description>
        ///             </item>
        ///             <item>
        ///                 <description>End</description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </summary>
        public Calendar CreateIcalFromJson(JsonObject jsonObjectOfEvent)
        {
            var ical = CalInit();
            var icalEvent = new CalendarEvent();
            
            var propertySetters = new Dictionary<Action<string>, string>
            {
                { value => icalEvent.Summary = value, "Summary" },
                { value => icalEvent.Location = value, "Location" },
                { value => icalEvent.Description = value, "Description" },
                { value => icalEvent.Start = new CalDateTime(DateTime.Parse(value)), "Start" },
                { value => icalEvent.End = new CalDateTime(DateTime.Parse(value)), "End" }
            };

            foreach (var pair in propertySetters)
            {
                TrySetProperty(pair.Key, () => jsonObjectOfEvent[pair.Value]?.ToString());
            }

            return ical;
        }
        
        private Calendar CalInit()
        {
            var ical = new Calendar();
            ical.AddTimeZone(TimeZoneInfo.Local);
            return ical;
        }

        private void TrySetProperty<T>(Action<T> setAction, Func<T?> getValue, T? defaultValue = default)
        {
            var value = getValue();
            if (value != null)
            {
                setAction(value);
            }
            else if (defaultValue != null)
            {
                setAction(defaultValue);
            }
        }
    }
}