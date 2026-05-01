using System;
using UnityEngine;

namespace NuclearDecline
{
    public sealed class MockAdsService : IAdsService
    {
        public bool IsInterstitialAvailable => true;
        public bool IsRewardedAvailable => true;

        public void ShowInterstitial(Action<AdResult> onComplete = null)
        {
            Debug.Log("[NuclearDecline] Mock interstitial ad shown.");
            onComplete?.Invoke(AdResult.SuccessResult());
        }

        public void ShowRewarded(string placementId, Action<AdResult> onComplete = null)
        {
            Debug.Log("[NuclearDecline] Mock rewarded ad shown. Placement: " + placementId);
            onComplete?.Invoke(AdResult.SuccessResult(placementId));
        }
    }
}
