# NuclearDecline Game Platform Bridge API

Developer-facing reference for `NuclearDecline.GamePlatformBridge`, the platform abstraction used by gameplay code to access ads, saves, localization, and platform lifecycle events without depending on Yandex Games, Plugin Your Games, or any SDK API directly.

## Purpose

`GamePlatformBridge` is the gameplay-facing entry point for platform features. It exposes stable C# interfaces and hides the active platform implementation behind `IPlatformService`.

Use it when gameplay needs to:

- show interstitial or rewarded ads;
- read or write persistent values;
- read or change the active language;
- notify the host platform about game readiness or gameplay state.

If no platform adapter is provided, the bridge initializes a safe `MockPlatformService` for editor and non-platform environments.

## GamePlatformBridge

Namespace: `NuclearDecline`

Static facade:

```csharp
IPlatformService GamePlatformBridge.Platform
IPlatformService GamePlatformBridge.Service
IAdsService GamePlatformBridge.Ads
ISaveService GamePlatformBridge.Saves
ILocalizationService GamePlatformBridge.Localization
bool GamePlatformBridge.IsInitialized

void GamePlatformBridge.Initialize(IPlatformService platformService = null)
```

Notes:

- Accessing `Platform`, `Service`, `Ads`, `Saves`, or `Localization` lazily initializes the bridge if needed.
- `Initialize()` uses `MockPlatformService` when `platformService` is `null`.
- A real initialized non-mock platform service is not replaced by later calls.
- `GamePlatformBridgeBootstrap` initializes the bridge before scene load.

## IPlatformService

Owns platform lifecycle and service instances.

```csharp
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
```

Use `GameReady()` when the game has finished loading and is ready for player interaction. Use `GameplayStart()` and `GameplayStop()` around active gameplay sessions so platform adapters can pause ads, analytics, or host-side state as needed.

## IAdsService

Platform-independent ad access.

```csharp
bool IsInterstitialAvailable { get; }
bool IsRewardedAvailable { get; }

void ShowInterstitial(Action<AdResult> onComplete = null);
void ShowRewarded(string placementId, Action<AdResult> onComplete = null);
```

Check availability before showing ads when the UI depends on it. Always handle the callback result; rewarded gameplay should grant rewards only when `result.IsSuccess` is true.

## ISaveService

Key-value save API for primitive values.

```csharp
void SaveInt(string key, int value);
int LoadInt(string key, int defaultValue = 0);

void SaveFloat(string key, float value);
float LoadFloat(string key, float defaultValue = 0f);

void SaveString(string key, string value);
string LoadString(string key, string defaultValue = "");

void SaveBool(string key, bool value);
bool LoadBool(string key, bool defaultValue = false);

bool HasKey(string key);
void DeleteKey(string key);

void Save();
void Load();

int GetInt(string key, int defaultValue = 0);
void SetInt(string key, int value);

float GetFloat(string key, float defaultValue = 0f);
void SetFloat(string key, float value);

string GetString(string key, string defaultValue = "");
void SetString(string key, string value);

bool GetBool(string key, bool defaultValue = false);
void SetBool(string key, bool value);
```

`Save*/Load*` and `Set*/Get*` are both available for compatibility. Prefer one style per system and call `Save()` after important mutations when data must be flushed immediately.

## ILocalizationService

Language selection and localized text lookup.

```csharp
string CurrentLanguage { get; }
string DefaultLanguage { get; }
string[] SupportedLanguages { get; }

event Action<string> OnLanguageChanged;
event Action<string> LanguageChanged;

void SetLanguage(string languageCode);
bool IsLanguageSupported(string languageCode);
string GetText(string key);
```

Use `IsLanguageSupported()` before setting a user-selected language. Subscribe to one language change event per component; both events expose the new language code.

## AdResult

Ad completion result returned by ad callbacks.

```csharp
enum AdResultStatus
{
    Success,
    Failed,
    Closed,
    Skipped
}

AdResultStatus Status { get; }
string PlacementId { get; }
string ErrorMessage { get; }

bool IsSuccess { get; }
bool IsFailed { get; }
bool IsClosed { get; }
bool IsSkipped { get; }
```

Factories and common values:

```csharp
AdResult.Completed
AdResult.Skipped
AdResult.Failed
AdResult.NotAvailable

AdResult.SuccessResult(string placementId = null)
AdResult.FailedResult(string placementId = null, string errorMessage = null)
AdResult.ClosedResult(string placementId = null)
AdResult.SkippedResult(string placementId = null)
```

## Usage Examples

Show a rewarded ad:

```csharp
using NuclearDecline;
using UnityEngine;

public void TryRevive()
{
    if (!GamePlatformBridge.Ads.IsRewardedAvailable)
        return;

    GamePlatformBridge.Ads.ShowRewarded("revive", result =>
    {
        if (result.IsSuccess)
            RevivePlayer();
        else
            Debug.Log("Rewarded ad did not complete: " + result);
    });
}
```

Save and load progress:

```csharp
using NuclearDecline;

public void SaveLevel(int level)
{
    GamePlatformBridge.Saves.SetInt("level", level);
    GamePlatformBridge.Saves.Save();
}

public int LoadLevel()
{
    GamePlatformBridge.Saves.Load();
    return GamePlatformBridge.Saves.GetInt("level", 1);
}
```

React to language changes:

```csharp
using NuclearDecline;

private void OnEnable()
{
    GamePlatformBridge.Localization.LanguageChanged += OnLanguageChanged;
}

private void OnDisable()
{
    GamePlatformBridge.Localization.LanguageChanged -= OnLanguageChanged;
}

private void OnLanguageChanged(string languageCode)
{
    title.text = GamePlatformBridge.Localization.GetText("title");
}
```

Notify platform lifecycle:

```csharp
using NuclearDecline;

private void Start()
{
    GamePlatformBridge.Platform.GameReady();
}

private void BeginRun()
{
    GamePlatformBridge.Platform.GameplayStart();
}

private void EndRun()
{
    GamePlatformBridge.Platform.GameplayStop();
}
```

## Boundaries

- Gameplay code may use `GamePlatformBridge` and the interfaces in this document.
- Gameplay code must not call Yandex Games, `YaGames`, `YandexGame`, `YG`, Plugin Your Games, or SDK-specific APIs directly.
- Platform-specific behavior belongs in an adapter that implements `IPlatformService`, `IAdsService`, `ISaveService`, and `ILocalizationService`.
- Keep SDK types, callbacks, and initialization details inside the adapter layer.
- Do not store scene, prefab, or serialized gameplay references inside platform adapters unless explicitly required.
- Mock implementations should remain safe for editor, tests, and non-Yandex builds.
