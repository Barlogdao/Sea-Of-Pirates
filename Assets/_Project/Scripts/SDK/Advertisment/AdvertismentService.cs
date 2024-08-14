using Project.Interfaces.SDK;
using Project.Systems.Pause;
using System;

namespace Project.SDK.Advertisment
{
    public abstract class AdvertismentService : IAdvertismentService
    {
        private readonly PauseService _pauseService;

        public AdvertismentService(PauseService pauseService)
        {
            _pauseService = pauseService;
        }

        public bool IsAdsPlaying { get; private set; } = false;

        public abstract void ShowInterstitialAd();

        public abstract void ShowRewardAd(Action onRewardedCallback);

        public abstract void ShowSticky();

        public abstract void HideSticky();


        protected void CloseAd()
        {
            IsAdsPlaying = false;
            _pauseService.Unpause();
        }

        protected void OpenAd()
        {
            IsAdsPlaying = true;
            _pauseService.Pause();
        }
    }
}