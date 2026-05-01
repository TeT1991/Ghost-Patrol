using System;

namespace NuclearDecline
{
    public interface ILocalizationService
    {
        string CurrentLanguage { get; }
        string DefaultLanguage { get; }
        string[] SupportedLanguages { get; }

        event Action<string> OnLanguageChanged;
        event Action<string> LanguageChanged;

        void SetLanguage(string languageCode);
        bool IsLanguageSupported(string languageCode);
        string GetText(string key);
    }
}
