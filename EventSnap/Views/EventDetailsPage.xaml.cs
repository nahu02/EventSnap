using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

public partial class EventDetailsPage : ContentPage
{
    public EventModel EventModel { get; set; } = new EventModel();

    private readonly IcalCreatorService _icalCreatorService;

    public EventDetailsPage() : this(MauiProgram.Services.GetRequiredService<IcalCreatorService>())
    {
    }

    public EventDetailsPage(IcalCreatorService icalService)
    {
        _icalCreatorService = icalService;

        InitializeComponent();
        dataForm.DataObject = EventModel;
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.Input;
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (dataForm.Validate())
        {
            var task = _icalCreatorService.AddEventToCalendarAsync(EventModel);
            ShowLoading("Adding event to calendar", task);

            EventModel = new EventModel();
            dataForm.DataObject = EventModel;
        }
    }

    private void ShowLoading(string message, Task task)
    {
        var loadingPage = new LoadingPage(message);

        Navigation.PushModalAsync(loadingPage);

        task.ContinueWith(t =>
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PopModalAsync();
            });
        });
    }
}