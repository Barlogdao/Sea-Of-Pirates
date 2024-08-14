using Agava.YandexGames;
using System;

namespace Project.SDK.InApp
{
    public interface IBillingService
    {
        void LoadProductCatalog(Action<CatalogProduct[]> onLoadCallback);
        void HandlePurchase(string id, Action onPurchaseCallback);
    }
}