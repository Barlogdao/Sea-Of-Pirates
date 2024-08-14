using System;

namespace Project.Interfaces.SDK
{
    public interface IAdvertismentService
    {
        bool IsAdsPlaying { get; }

        void ShowInterstitialAd();
        void ShowRewardAd(Action onRewardedCallback);
        void ShowSticky();
        void HideSticky();
    }
}