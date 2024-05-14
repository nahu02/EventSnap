using CommunityToolkit.Mvvm.Messaging;
using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

/// <summary>
/// Represents a page that displays the details of an event.
/// It also shows a Save button to add the event to the calendar, using the IcalCreatorService.
/// </summary>
[QueryProperty(nameof(SharedText), "sharedText")]
public partial class EventDetailsPage : ContentPage
{
    public EventModel EventModel
    {
        get => (EventModel)dataForm.DataObject;
        set => dataForm.DataObject = value;

    }

    public string SharedText
    {
        set => OnSharedTextReceivedAsync(value);
    }

    private readonly IcalCreatorService _icalCreatorService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDetailsPage"/> class.
    /// Gets the IcalCreatorService from the service provider.
    /// </summary>
    public EventDetailsPage() : this(MauiProgram.Services.GetRequiredService<IcalCreatorService>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDetailsPage"/> class.
    /// </summary>
    /// <param name="icalService">The IcalCreatorService instance.</param>
    public EventDetailsPage(IcalCreatorService icalService)
    {
        _icalCreatorService = icalService;

        InitializeComponent();

        EventModel = new EventModel();
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.Input;

        WeakReferenceMessenger.Default.Register<string>(this, (_, msg) => SharedText = msg);
    }

    /// <summary>
    /// Handles the event when shared text is received.
    /// Calls the AiCommunicatorService to process the shared text and get an event model, which is then displayed.
    /// A <see cref="LoadingPage"/> is shown while the event is being processed.
    /// </summary>
    /// <param name="sharedText">The shared text received.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task OnSharedTextReceivedAsync(string sharedText)
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


    /// <summary>
    /// Event handler for the settings button click event.
    /// Navigates to the SettingsPage.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SettingsPage", animate: true);
    }

    /// <summary>
    /// Event handler for the save button click event.
    /// Calls the IcalCreatorService to add the event to the calendar.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
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

    /// <summary>
    /// Displays a <see cref="LoadingPage"/> with the specified message while the task is running.
    /// </summary>
    /// <param name="message">The message to display on the loading page.</param>
    /// <param name="task">The task that determines when the loading page should be removed.</param>
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