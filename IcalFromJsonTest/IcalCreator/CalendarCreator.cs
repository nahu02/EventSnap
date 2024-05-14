using System.Text.Json.Nodes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;

namespace IcalCreator
{
    public class CalendarCreator : ICalendarCreator
    {
        public Calendar CreateIcal(CalendarEventProperties eventProperties)
        {
            ArgumentNullException.ThrowIfNull(eventProperties);

            var ical = CalInit();
            var icalEvent = new CalendarEvent
            {
                Summary = eventProperties.Summary,
                Description = eventProperties.Description,
                Location = eventProperties.Location,
                Start = eventProperties.Start != null ? new CalDateTime(DateTime.Parse(eventProperties.Start)) : null,
                End = eventProperties.End != null ? new CalDateTime(DateTime.Parse(eventProperties.End)) : null
            };

            ical.Events.Add(icalEvent);
            return ical;
        }

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
            var eventProps = new CalendarEventProperties();

            var propertySetters = new Dictionary<Action<string>, string>
            {
                { value => eventProps.Summary = value, "Summary" },
                { value => eventProps.Location = value, "Location" },
                { value => eventProps.Description = value, "Description" },
                { value => eventProps.Start = value, "Start" },
                { value => eventProps.End = value, "End" }
            };

            foreach (var (setPropertyAction, jsonField) in propertySetters)
            {
                TrySetProperty(setPropertyAction, () => jsonObjectOfEvent[jsonField]?.ToString());
            }

            return CreateIcal(eventProps);
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