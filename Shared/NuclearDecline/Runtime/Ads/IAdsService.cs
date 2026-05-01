using System;

namespace NuclearDecline
{
    public interface IAdsService
    {
        bool IsInterstitialAvailable { get; }
        bool IsRewardedAvailable { get; }

        void ShowInterstitial(Action<AdResult> onComplete = null);
        void ShowRewarded(string placementId, Action<AdResult> onComplete = null);
    }
}
