using EventSnap.Models;
using EventSnap.Services;

namespace EventSnap;

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
    }
}