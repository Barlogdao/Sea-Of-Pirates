using System;
using Project.Interfaces.SDK;
using Project.Systems.Pause;
using UnityEngine;

namespace Project.SDK.Advertisment
{
    public class MockAdvertismentService : AdvertismentService, IAdvertismentService
    {
        public MockAdvertismentService(PauseService pauseService)
            : base(pauseService)
        {
        }

        public override void ShowInterstitialAd()
        {
            Debug.Log("Interstitial Shown");
        }

        public override void ShowRewardAd(Action onRewardedCallback)
        {
            onRewardedCallback();
        }

        public override void ShowSticky()
        {
            Debug.Log("Sticky Shown");

        }

        public override void HideSticky()
        {
            Debug.Log("Sticky Hided");
        }
    }
}
