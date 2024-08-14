using System.Linq;
using Agava.YandexGames;
using Project.Configs.ShopItems;
using Project.Interfaces.Data;
using Project.SDK.InApp;
using Project.Systems.Shop.Items;
using Project.UI.Shop;
using Zenject;

namespace Project.Systems.Shop
{
    public class ShopSystem : IInitializable
    {
        private readonly IPlayerStorage _playerStorage;
        private readonly IBillingProvider _billingProvider;
        private readonly ShopItemFactory _shopItemfactory;
        private readonly ShopItemsConfigs _shopItemsConfigs;
        private readonly ShopWindow _shopWindow;
        private readonly ShopButton _shopButton;

        private bool _isItemsLoaded = false;

        public ShopSystem(
            IPlayerStorage playerStorage,
            IBillingProvider billingProvider,
            ShopItemFactory shopItemfactory,
            ShopItemsConfigs shopItemsConfigs,
            ShopWindow shopWindow,
            ShopButton shopButtom)
        {
            _playerStorage = playerStorage;
            _billingProvider = billingProvider;
            _shopItemfactory = shopItemfactory;
            _shopItemsConfigs = shopItemsConfigs;
            _shopWindow = shopWindow;
            _shopButton = shopButtom;
        }

        public void Initialize()
        {
            _shopButton.Show(OpenShop);
        }

        private void OpenShop()
        {
            _shopWindow.Open();

            if (_isItemsLoaded)
                return;

            LoadShopItems();
        }
        private void LoadShopItems()
        {
            LoadGameItems();
            _billingProvider.LoadProductCatalog(LoadInAppItems);

            _isItemsLoaded = true;
        }

        private void LoadGameItems()
        {
            foreach (GameItemConfig config in _shopItemsConfigs.GameItemsConfigs)
            {
                GameItem item = _shopItemfactory.Create(config);
                _shopWindow.CreateItemSlot(item, () => BuyItem(item));
            }
        }

        private void LoadInAppItems(CatalogProduct[] products)
        {
            foreach (var config in _shopItemsConfigs.InAppItemsConfigs)
            {
                CatalogProduct itemData = products.FirstOrDefault(p => p.id == config.ID);

                if (itemData == null)
                    continue;

                InAppItem item = _shopItemfactory.Create(config, itemData);

                if (item.IsAvaliable == false)
                    continue;

                _shopWindow.CreateItemSlot(item, () => BuyItem(item));
            }
        }

        private void BuyItem(InAppItem item)
        {
            _billingProvider.HandlePurchase(item.ID, () =>
            {
                GetShopItem(item);
                _shopWindow.CheckSlots();
            });
        }

        private void BuyItem(GameItem item)
        {
            if (_playerStorage.TrySpendResource(item.Price))
                GetShopItem(item);
        }

        private void GetShopItem(ShopItem item)
        {
            item.Get();
        }
    }
}