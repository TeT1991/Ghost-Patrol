using UnityEngine;

namespace NuclearDecline
{
    public static class GamePlatformBridge
    {
        private static IPlatformService service;

        public static IPlatformService Platform
        {
            get
            {
                EnsureInitialized();
                return service;
            }
        }

        public static IPlatformService Service => Platform;

        public static IAdsService Ads => Platform.Ads;
        public static ISaveService Saves => Platform.Saves;
        public static ILocalizationService Localization => Platform.Localization;

        public static bool IsInitialized => service != null && service.IsInitialized;

        public static void Initialize(IPlatformService platformService = null)
        {
            if (platformService != null && service != platformService)
            {
                if (service != null && service.IsInitialized && !(service is MockPlatformService))
                    return;

                service = platformService;
            }
            else if (service != null && service.IsInitialized)
            {
                return;
            }

            if (service == null)
                service = new MockPlatformService();

            service.Initialize();

            Debug.Log("[NuclearDecline] GamePlatformBridge initialized: " + service.PlatformName);
        }

        private static void EnsureInitialized()
        {
            if (service == null || !service.IsInitialized)
                Initialize();
        }
    }
}
