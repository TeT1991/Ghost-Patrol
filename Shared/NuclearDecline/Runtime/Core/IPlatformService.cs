using System;

namespace NuclearDecline
{
    public interface IPlatformService
    {
        bool IsReady { get; }
        bool IsInitialized { get; }
        string PlatformName { get; }
        string PlayerId { get; }

        IAdsService Ads { get; }
        ILocalizationService Localization { get; }
        ISaveService Saves { get; }

        event Action OnReady;

        void Initialize();
        void GameReady();
        void GameplayStart();
        void GameplayStop();
    }
}
