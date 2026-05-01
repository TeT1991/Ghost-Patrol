using UnityEngine;

namespace NuclearDecline
{
    public sealed class GamePlatformBridgeBootstrap : MonoBehaviour
    {
        private static bool bootstrapped;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (bootstrapped)
                return;

            bootstrapped = true;

            GameObject bootstrapObject = new GameObject(nameof(GamePlatformBridgeBootstrap));
            DontDestroyOnLoad(bootstrapObject);
            bootstrapObject.AddComponent<GamePlatformBridgeBootstrap>();

            GamePlatformBridge.Initialize();
        }

        private void Awake()
        {
            GamePlatformBridge.Initialize();
        }
    }
}
