namespace EventSnap.Services
{
    public class SettingsService
    {
        public string OpenAiApiKey
        {
            get => Preferences.Get(nameof(OpenAiApiKey), string.Empty);
            set
            {
                System.Diagnostics.Debug.WriteLine($"openai key set to {value}");
                Preferences.Set(nameof(OpenAiApiKey), value);
            }
        }

        public string OpenAiModel
        {
            get
            {
                var val = Preferences.Get(nameof(OpenAiModel), "gpt-4-turbo");
                System.Diagnostics.Debug.WriteLine($"openai model get {val}");
                return val;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine($"openai model set to {value}");
                Preferences.Set(nameof(OpenAiModel), value);
            }
        }
    }
}
