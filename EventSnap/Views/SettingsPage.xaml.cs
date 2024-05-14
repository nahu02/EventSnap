using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

/// <summary>
/// Represents the settings page of the application.
/// </summary>
public partial class SettingsPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class.
    /// Gets the settings service from the service provider.
    /// </summary>
    public SettingsPage() : this(MauiProgram.Services.GetRequiredService<SettingsService>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsPage"/> class with the specified settings service.
    /// </summary>
    /// <param name="settingsService">The settings service to use.</param>
    public SettingsPage(SettingsService settingsService)
    {
        InitializeComponent();
        dataForm.DataObject = new SettingsModel(settingsService);
        dataForm.PickerSourceProvider = new SettingsModel.SettingsComboDataProvider();
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.LostFocus;
    }

    /// <summary>
    /// Handles the event when the save button is clicked.
    /// Navigates to the event details page.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EventDetailsPage", animate: true);
    }
}