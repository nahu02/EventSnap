using System.Text.Json.Nodes;
using Ical.Net.Serialization;
using IcalCreator;

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

            ICalendarCreator creator = new CalendarCreator();
            var ical = creator.CreateIcalFromJson(jsonObject);

            var serializer = new CalendarSerializer();
            Console.WriteLine(serializer.SerializeToString(ical));
        }
    }
}