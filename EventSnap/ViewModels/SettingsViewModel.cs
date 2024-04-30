using CommunityToolkit.Mvvm.ComponentModel;
using EventSnap.Services;
using System.ComponentModel.DataAnnotations;

namespace EventSnap.ViewModels
{
    public partial class SettingsViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required]
        [Length(51, 51, ErrorMessage = "The API key must be 51 characters long.")]
        private string _openAiApiKey;

        [ObservableProperty]
        [Required]
        private string _openAiModel;

        public List<string> OpenAiModels { get; set; } = ["gpt-4-turbo", "gpt-4", "gpt-3.5-turbo"];

        private readonly SettingsService _settingsService;

        public SettingsViewModel() : this(MauiProgram.Services.GetRequiredService<SettingsService>()) { }

        public SettingsViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;

            OpenAiApiKey = _settingsService.OpenAiApiKey;
            OpenAiModel = _settingsService.OpenAiModel;
        }

        partial void OnOpenAiApiKeyChanged(string? oldValue, string newValue)
        {
            _settingsService.OpenAiApiKey = newValue;
        }

        partial void OnOpenAiModelChanged(string? oldValue, string newValue)
        {
            _settingsService.OpenAiModel = newValue;
        }
    }
}
