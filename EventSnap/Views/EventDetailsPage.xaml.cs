using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

public partial class EventDetailsPage : ContentPage
{
    public EventModel EventModel { get; set; } = new EventModel();

    public EventDetailsPage() : this(MauiProgram.Services.GetRequiredService<SettingsService>())
    {
    }

    public EventDetailsPage(SettingsService settingsService)
    {
        InitializeComponent();
        dataForm.DataObject = EventModel;
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.LostFocus;
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (dataForm.Validate())
        {
            System.Diagnostics.Debug.WriteLine("DataForm is valid, implement create logic");
        }
    }
}