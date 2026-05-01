using System;
using UnityEngine;

namespace NuclearDecline
{
    public sealed class MockPlatformService : IPlatformService
    {
        public event Action OnReady;

        public bool IsReady { get; private set; }
        public bool IsInitialized { get; private set; }
        public string PlatformName => "Mock";
        public string PlayerId => "mock_player";

        public IAdsService Ads { get; private set; }
        public ILocalizationService Localization { get; private set; }
        public ISaveService Saves { get; private set; }

        public void Initialize()
        {
            if (IsInitialized)
                return;

            Saves = new MockSaveService();
            Localization = new MockLocalizationService(Saves);
            Ads = new MockAdsService();

            IsInitialized = true;
            IsReady = true;
            Debug.Log("[NuclearDecline] Mock platform service ready.");
            OnReady?.Invoke();
        }

        public void GameReady()
        {
            Debug.Log("[NuclearDecline] Mock GameReady.");
        }

        public void GameplayStart()
        {
            Debug.Log("[NuclearDecline] Mock GameplayStart.");
        }

        public void GameplayStop()
        {
            Debug.Log("[NuclearDecline] Mock GameplayStop.");
        }
    }
}
