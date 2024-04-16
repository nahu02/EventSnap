using System.Text.Json.Nodes;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace IcalTest;

class Program
{
    static void Main(string[] args)
    {
        var jsonRaw =
            "{\n  \"Summary\": \"Exam\",\n  \"Start\": \"4/17/2024 9:00:00 AM\",\n  \"End\": \"4/17/2024 10:15:00 AM\",\n  \"Description\": \"You may use a calculator. Good luck!\"\n}";
        var jsonObject = JsonArray.Parse(jsonRaw).AsObject();

        foreach (var kv in jsonObject)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }

        var icalEvent = new CalendarEvent();

        try
        {
            icalEvent.Summary = jsonObject["Summary"].ToString();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e); // we leave that one blank and move on
        }
        try
        {
            icalEvent.Location = jsonObject["Location"].ToString();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e); // we leave that one blank and move on
        }
        try
        {
            icalEvent.Start = new CalDateTime(DateTime.Parse(jsonObject["Start"].ToString()));
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e); // we leave that one blank and move on
        }
        try
        {
            icalEvent.End = new CalDateTime(DateTime.Parse(jsonObject["End"].ToString()));
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e); // we leave that one blank and move on
        }
        try
        {
            icalEvent.Description = jsonObject["Description"].ToString();
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e); // we leave that one blank and move on
        }

        var ical = new Calendar();
        ical.Events.Add(icalEvent);
        ical.AddTimeZone(TimeZoneInfo.Local);

        var serializer = new CalendarSerializer();
        Console.WriteLine(serializer.SerializeToString(ical));
    }
}
