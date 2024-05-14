using DevExpress.Maui.DataForm;
using EventSnap.Services;

namespace EventSnap.Models
{
    /// <summary>
    /// Represents the settings model for the application.
    /// </summary>
    public class SettingsModel
    {
        [DataFormDisplayOptions(LabelText = "OpenAI API Key", GroupName = "OpenAI", LabelPosition = DataFormLabelPosition.Top)]
        public string OpenAiApiKey
        {
            get => _settingsService.OpenAiApiKey;
            set => _settingsService.OpenAiApiKey = value;
        }

        [DataFormDisplayOptions(LabelText = "Model to use", GroupName = "OpenAI", LabelPosition = DataFormLabelPosition.Top)]
        [DataFormComboBoxEditor(PickerShowMode = DevExpress.Maui.Editors.DropDownShowMode.BottomSheet)]
        public string OpenAiModelToUse
        {
            get => _settingsService.OpenAiModel;
            set => _settingsService.OpenAiModel = value;
        }

        private SettingsService _settingsService;

        public SettingsModel(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Provides the data source for the picker control in the settings model.
        /// </summary>
        public class SettingsComboDataProvider : IPickerSourceProvider
        {
            /// <summary>
            /// Gets the data source for <see cref="OpenAiModelToUse"/>. 
            /// For that property, it returns a list of available OpenAI text models.
            /// For any other property, it throws a <see cref="NotImplementedException"/>.
            /// </summary>
            /// <param name="propertyName">The name of the property. (Expected: "OpenAiModelToUse")</param>
            /// <returns>The data source for the specified property.</returns>
            public System.Collections.IEnumerable GetSource(string propertyName)
            {
                if (propertyName == nameof(OpenAiModelToUse))
                {
                    var availableOpenaiTextModels = new List<string>
                    {
                        "gpt-4-turbo",
                        "gpt-4",
                        "gpt-3.5-turbo",
                    };
                    return availableOpenaiTextModels;
                }

                throw new NotImplementedException($"Source not found for {propertyName}");
            }
        }

    }
}
