namespace IcalCreator
{
    /// <summary>
    ///     Data class for storing properties of a calendar event.
    ///     Every property is nullable, so that the class can be used to store partial data.
    ///     Every property is a string, and is expected to be parsed by the user of the class.
    /// </summary>
    public record CalendarEventProperties
    {
        public string? Summary { get; set; }

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Start { get; set; }

        public string? End { get; set; }
    }
}