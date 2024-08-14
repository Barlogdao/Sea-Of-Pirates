using Agava.YandexGames;
using Project.Configs.ShopItems;
using Project.SDK.Advertisment;

namespace Project.Systems.Shop.Items
{
    public class AddRemovalItem : InAppItem
    {
        private readonly AdvertismentController _advertismentController;

        public AddRemovalItem(
            AdvertismentController advertismentController,
            AddRemovalConfig config,
            CatalogProduct itemData)
            : base(config, itemData)
        {
            _advertismentController = advertismentController;
        }

        public override bool IsAvaliable => _advertismentController.IsAddActive;

        public override string AmountText => string.Empty;

        public override void Get()
        {
            _advertismentController.RemoveAd();
        }
    }
}