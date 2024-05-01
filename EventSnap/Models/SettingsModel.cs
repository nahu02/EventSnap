﻿using DevExpress.Maui.DataForm;
using EventSnap.Services;

namespace EventSnap.Models
{
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

        public class SettingsComboDataProvider : IPickerSourceProvider
        {
            System.Collections.IEnumerable IPickerSourceProvider.GetSource(string propertyName)
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
