using System.Collections.Generic;
using System.Linq;
using Project.Configs.GameResources;
using Project.Interfaces.Data;
using Project.Systems.Stats;

namespace Project.Systems.Data
{
    public class PlayerStorage : IPlayerStorage
    {
        private readonly IResourceStorageProvider _resourceStorageProvider;
        private Dictionary<GameResource, int> _storage;

        public PlayerStorage(IResourceStorageProvider provider)
        {
            _resourceStorageProvider = provider;
            _storage = _resourceStorageProvider.LoadStorage();
        }

        public void AddResource(GameResource gameResource, int amount)
        {
            _storage[gameResource] += amount;
            SaveResources();
        }

        public void AddResource(GameResourceAmount gameResourceAmount)
        {
            AddResource(gameResourceAmount.Resource, gameResourceAmount.Amount);
        }

        public void AddResource(List<GameResourceAmount> gameResourcesAmounts)
        {
            foreach (var gameResourceAmount in gameResourcesAmounts)
            {
                AddResource(gameResourceAmount.Resource, gameResourceAmount.Amount);
            }
        }

        public bool TrySpendResource(GameResource gameResource, int amount)
        {
            if (CanSpend(gameResource, amount))
            {
                SpendResource(gameResource, amount);
                return true;
            }

            return false;
        }

        public bool TrySpendResource(GameResourceAmount gameResourceAmount)
        {
            return TrySpendResource(gameResourceAmount.Resource, gameResourceAmount.Amount);
        }

        public bool TrySpendResource(List<GameResourceAmount> gameResourcesAmounts)
        {
            if (CanSpend(gameResourcesAmounts) == false)
            {
                return false;
            }

            foreach (var gameResourceAmount in gameResourcesAmounts)
            {
                SpendResource(gameResourceAmount.Resource, gameResourceAmount.Amount);
            }

            return true;
        }

        public bool CanSpend(GameResource gameResource, int amount)
        {
            return _storage[gameResource] >= amount;
        }

        public bool CanSpend(GameResourceAmount gameResourceAmount)
        {
            return _storage[gameResourceAmount.Resource] >= gameResourceAmount.Amount;
        }

        public bool CanSpend(List<GameResourceAmount> gameResourcesAmounts)
        {
            return gameResourcesAmounts.All(CanSpend);
        }

        private void SpendResource(GameResource gameResource, int amount)
        {
            _storage[gameResource] -= amount;
            SaveResources();
        }
        
        private void SaveResources()
        {
            _resourceStorageProvider.UpdateStorage();
        }
    }
}