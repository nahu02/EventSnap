using System.Text.Json.Nodes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace IcalTest
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var jsonRaw =
                "{\n  \"Summary\": \"Exam\",\n  \"Start\": \"4/17/2024 9:00:00 AM\",\n  \"End\": \"4/17/2024 10:15:00 AM\",\n  \"Description\": \"You may use a calculator. Good luck!\"\n}";
            var jsonObject = JsonNode.Parse(jsonRaw)?.AsObject();

            foreach (var kv in jsonObject)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            var icalEvent = new CalendarEvent();

            var propertySetters = new Dictionary<Action<string>, string>
            {
                { value => icalEvent.Summary = value, "Summary" },
                { value => icalEvent.Location = value, "Location" },
                { value => icalEvent.Start = new CalDateTime(DateTime.Parse(value)), "Start" },
                { value => icalEvent.End = new CalDateTime(DateTime.Parse(value)), "End" },
                { value => icalEvent.Description = value, "Description" }
            };

            foreach (var pair in propertySetters)
            {
                TrySetProperty(pair.Key, () => jsonObject[pair.Value]?.ToString());
            }

            var ical = new Calendar();
            ical.Events.Add(icalEvent);
            ical.AddTimeZone(TimeZoneInfo.Local);

            var serializer = new CalendarSerializer();
            Console.WriteLine(serializer.SerializeToString(ical));
        }

        private static void TrySetProperty<T>(Action<T> setAction, Func<T?> getValue, T? defaultValue = default)
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