using Project.Interfaces.SDK;
using Project.Systems.Pause;
using System;

namespace Project.SDK.Advertisment
{
    public class YandexAdvertismentService : AdvertismentService, IAdvertismentService
    {
        public YandexAdvertismentService(PauseService pauseService)
            : base(pauseService)
        {
        }

        public override void ShowInterstitialAd()
        {
            Agava.YandexGames.InterstitialAd.Show(OpenAd, OnCloseInterstitial);

            void OnCloseInterstitial(bool wasShown)
            {
                CloseAd();
            }
        }

        public override void ShowRewardAd(Action onRewardedCallback)
        {
            Agava.YandexGames.VideoAd.Show(OpenAd, () => onRewardedCallback(), CloseAd);
        }

        public override void HideSticky()
            => Agava.YandexGames.StickyAd.Hide();

        public override void ShowSticky()
            => Agava.YandexGames.StickyAd.Show();
    }
}
