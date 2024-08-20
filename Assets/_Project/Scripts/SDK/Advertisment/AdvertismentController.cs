using Project.Interfaces.Data;
using Project.Interfaces.SDK;

namespace Project.SDK.Advertisment
{
    public class AdvertismentController
    {
        private readonly IAdvertismentData _addData;
        private readonly IAdvertismentService _advertismentService;

        public AdvertismentController(IAdvertismentData addData, IAdvertismentService advertismentService)
        {
            _addData = addData;
            _advertismentService = advertismentService;
            HandleAd();
        }

        public bool IsAddActive => _addData.IsAdActive;

        public void ShowIntersticialAd()
        {
            if (IsAddActive)
                _advertismentService.ShowInterstitialAd();
        }

        public void RemoveAd()
        {
            _addData.IsAdActive = false;
            _addData.Save();

            HandleAd();
        }

        private void HandleAd()
        {
            if (IsAddActive)
                _advertismentService.ShowSticky();
            else
                _advertismentService.HideSticky();
        }
    }
}