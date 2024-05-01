using DevExpress.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace EventSnap.Models
{
    public class EventModel
    {
        [DataFormDisplayOptions(LabelPosition = DataFormLabelPosition.Top)]
        [Required(ErrorMessage = "You cannot have an event without a title")]
        public string Title { get; set; }

        [DataFormDisplayOptions(LabelPosition = DataFormLabelPosition.Top)]
        public string Description { get; set; }

        [DataFormDisplayOptions(LabelPosition = DataFormLabelPosition.Top)]
        public string Location { get; set; }


        [DataFormDisplayOptions(LabelText = "Start Date", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormDateEditor]
        public DateTime StartDate { get; set; } = DateTime.Today.Date;

        [DataFormDisplayOptions(LabelText = "Start Time", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormTimeEditor]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [DataFormDisplayOptions(LabelText = "End Date", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormDateEditor]
        public DateTime EndDate { get; set; } = DateTime.Today.Date;

        [DataFormDisplayOptions(LabelText = "End Time", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormTimeEditor]
        public DateTime EndTime { get; set; } = DateTime.Now;



        [DataFormDisplayOptions(SkipAutoGenerating = true)]
        public DateTime EndDateTime
        {
            get => new(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hour, EndTime.Minute, 0);
            set
            {
                EndDate = value.Date;
                EndTime = value;
            }
        }

        [DataFormDisplayOptions(SkipAutoGenerating = true)]
        public DateTime StartDateTime
        {
            get => new(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hour, StartTime.Minute, 0);
            set
            {
                StartDate = value.Date;
                StartTime = value;
            }
        }

    }
}
