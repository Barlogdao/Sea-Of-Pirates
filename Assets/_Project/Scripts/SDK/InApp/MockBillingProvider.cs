﻿using Agava.YandexGames;
using Project.Configs.ShopItems;
using System;
using System.Collections.Generic;

namespace Project.SDK.InApp
{
    public class MockBillingProvider : IBillingProvider
    {
        private readonly ShopItemsConfigs _shopItemsConfigs;

        public MockBillingProvider(ShopItemsConfigs shopItemsConfigs)
        {
            _shopItemsConfigs = shopItemsConfigs;
        }

        public void LoadProductCatalog(Action<CatalogProduct[]> onLoadCallback)
        {
            List<CatalogProduct> products = new List<CatalogProduct>();

            foreach (var config in _shopItemsConfigs.InAppItemsConfigs)
            {
                products.Add(new CatalogProduct() { id = config.ID, price = "4 YAN" });
            }

            onLoadCallback(products.ToArray());
        }

        public void HandlePurchase(string id, Action onPurchaseCallback)
        {
            onPurchaseCallback();
        }
    }
}