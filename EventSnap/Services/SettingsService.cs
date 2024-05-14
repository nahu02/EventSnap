namespace EventSnap.Services
{
    /// <summary>
    /// Represents a service that provides access to application settings.
    /// It uses MAUI Preferences to store/retrieve settings.
    /// </summary>
    public class SettingsService
    {
        /// <summary>
        /// Gets or sets the OpenAI API key.
        /// </summary>
        public string OpenAiApiKey
        {
            get => Preferences.Get(nameof(OpenAiApiKey), string.Empty);
            set => Preferences.Set(nameof(OpenAiApiKey), value);
        }

        /// <summary>
        /// Gets or sets the OpenAI model.
        /// </summary>
        public string OpenAiModel
        {
            get => Preferences.Get(nameof(OpenAiModel), "gpt-4-turbo");
            set => Preferences.Set(nameof(OpenAiModel), value);
        }
    }
}
