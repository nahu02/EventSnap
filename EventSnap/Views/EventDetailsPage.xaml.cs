using CommunityToolkit.Mvvm.Messaging;
using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

public partial class EventDetailsPage : ContentPage
{
    public EventModel EventModel
    {
        get => (EventModel)dataForm.DataObject;
        set => dataForm.DataObject = value;

    }

    private readonly IcalCreatorService _icalCreatorService;

    public EventDetailsPage() : this(MauiProgram.Services.GetRequiredService<IcalCreatorService>())
    {
    }

    public EventDetailsPage(IcalCreatorService icalService)
    {
        _icalCreatorService = icalService;

        InitializeComponent();

        EventModel = new EventModel();
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.Input;

        WeakReferenceMessenger.Default.Register<string>(this, (_, msg) => OnSharedTextReceivedAsync(msg));
    }

    public async Task OnSharedTextReceivedAsync(string sharedText)
    {
        try
        {
            var task = MauiProgram.Services.GetRequiredService<AiCommunicatorService>().GetEventFromNaturalLanguageTextAsync(sharedText);
            ShowLoading("Processing event", task);
            EventModel = await task;
        }
        catch (AggregateException e)
        {
            await DisplayAlert("Problem with processing the event", e.Message, "OK");
        }

        System.Diagnostics.Debug.WriteLine($"Event!\n\tTitle: {EventModel.Title}\n\tDescription: {EventModel.Description}\n\tLocation: {EventModel.Location}\n\tStart: {EventModel.StartDateTime}\n\tEnd: {EventModel.EndDateTime}");
    }


    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SettingsPage", animate: true);
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