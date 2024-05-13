using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap.Views;

public partial class SettingsPage : ContentPage
{

    public SettingsPage() : this(MauiProgram.Services.GetRequiredService<SettingsService>())
    {
    }

    public SettingsPage(SettingsService settingsService)
    {
        InitializeComponent();
        dataForm.DataObject = new SettingsModel(settingsService);
        dataForm.PickerSourceProvider = new SettingsModel.SettingsComboDataProvider();
        dataForm.CommitMode = DevExpress.Maui.DataForm.CommitMode.LostFocus;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//EventDetailsPage", animate: true);
    }
}