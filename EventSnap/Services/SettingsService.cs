namespace EventSnap.Services
{
    public class SettingsService
    {
        public string OpenAiApiKey
        {
            get => Preferences.Get(nameof(OpenAiApiKey), string.Empty);
            set => Preferences.Set(nameof(OpenAiApiKey), value);
        }

        public string OpenAiModel
        {
            get => Preferences.Get(nameof(OpenAiModel), "gpt-4-turbo");
            set => Preferences.Set(nameof(OpenAiModel), value);
        }
    }
}
