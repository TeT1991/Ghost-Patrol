using System;
using System.Collections.Generic;
using UnityEngine;

namespace NuclearDecline
{
    public sealed class MockLocalizationService : ILocalizationService
    {
        private const string LanguageKey = "NuclearDecline.Language";
        private static readonly string[] Languages = { "ru", "en" };

        private readonly ISaveService saveService;
        private readonly Dictionary<string, Dictionary<string, string>> textByLanguage =
            new Dictionary<string, Dictionary<string, string>>();

        public event Action<string> OnLanguageChanged;
        public event Action<string> LanguageChanged;

        public string CurrentLanguage { get; private set; }
        public string DefaultLanguage => "ru";
        public string[] SupportedLanguages => (string[])Languages.Clone();

        public MockLocalizationService(ISaveService saveService)
        {
            this.saveService = saveService;
            CurrentLanguage = NormalizeLanguage(saveService.LoadString(LanguageKey, DefaultLanguage));
        }

        public void SetLanguage(string languageCode)
        {
            languageCode = NormalizeLanguage(languageCode);

            if (CurrentLanguage == languageCode)
                return;

            CurrentLanguage = languageCode;
            saveService.SaveString(LanguageKey, CurrentLanguage);
            saveService.Save();

            Debug.Log("[NuclearDecline] Mock language changed: " + CurrentLanguage);
            OnLanguageChanged?.Invoke(CurrentLanguage);
            LanguageChanged?.Invoke(CurrentLanguage);
        }

        public bool IsLanguageSupported(string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
                return false;

            for (int i = 0; i < Languages.Length; i++)
            {
                if (Languages[i] == languageCode)
                    return true;
            }

            return false;
        }

        public string GetText(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            string text;
            if (TryGetText(CurrentLanguage, key, out text))
                return text;

            if (TryGetText(DefaultLanguage, key, out text))
                return text;

            return key;
        }

        private bool TryGetText(string languageCode, string key, out string text)
        {
            text = null;

            Dictionary<string, string> table;
            if (!textByLanguage.TryGetValue(languageCode, out table))
                return false;

            return table.TryGetValue(key, out text);
        }

        private string NormalizeLanguage(string languageCode)
        {
            if (IsLanguageSupported(languageCode))
                return languageCode;

            return DefaultLanguage;
        }
    }
}
