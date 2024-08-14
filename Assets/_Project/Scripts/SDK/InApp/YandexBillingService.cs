using Agava.YandexGames;
using System;

namespace Project.SDK.InApp
{
    public class YandexBillingService : IBillingService
    {
        public void LoadProductCatalog(Action<CatalogProduct[]> onLoadCallback)
        {
            Billing.GetProductCatalog(productCatalogRespose => onLoadCallback(productCatalogRespose.products));
        }

        public void HandlePurchase(string itemID, Action onPurchaseCallback)
        {
            Billing.PurchaseProduct(itemID, (purchaseProductResponse) =>
            {
                Billing.ConsumeProduct(purchaseProductResponse.purchaseData.purchaseToken, () =>
                {
                    onPurchaseCallback();
                });
            });
        }
    }
}