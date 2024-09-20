using System;
using System.Collections.Generic;
using System.Linq;
using Ami.BroAudio;
using Project.Configs.Game;
using Project.Configs.GameResources;
using Project.Interfaces.Audio;
using Project.Interfaces.Data;
using Project.Interfaces.Storage;
using Project.Systems.Data;
using UnityEngine;

namespace Project.Systems.Storage
{
    public class PlayerStorage : IPlayerStorage, IStorageNotifier
    {
        private readonly IResourceStorageProvider _resourceStorageProvider;
        private readonly IAudioService _audioService;
        private readonly Dictionary<GameResource, int> _storage;
        private readonly SoundID _earnResourceSound;

        public event Action<GameResource, int> ResourceAmountChanged;

        public PlayerStorage(IResourceStorageProvider provider, IAudioService audioService, GameConfig config)
        {
            _resourceStorageProvider = provider;
            _audioService = audioService;
            _storage = _resourceStorageProvider.LoadStorage();
            _earnResourceSound = config.EarnResourceSound;
        }

        public int GetResourceAmount(GameResource gameResource)
        {
            return _storage[gameResource];
        }

        public void AddResource(GameResource gameResource, int amount)
        {
            AddToStorage(gameResource, amount);

            _audioService.PlaySound(_earnResourceSound);
            SaveResources();
        }

        public void AddResource(GameResourceAmount gameResourceAmount)
        {
            AddResource(gameResourceAmount.Resource, gameResourceAmount.Amount);
        }

        public void AddResource(List<GameResourceAmount> gameResourcesAmounts)
        {
            foreach (GameResourceAmount gameResourceAmount in gameResourcesAmounts)
                AddToStorage(gameResourceAmount.Resource,gameResourceAmount.Amount);

            _audioService.PlaySound(_earnResourceSound);
            SaveResources();
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

        private void AddToStorage(GameResource gameResource, int amount)
        {
            _storage[gameResource] += amount;
            ResourceAmountChanged?.Invoke(gameResource, _storage[gameResource]);
        }

        private void SpendResource(GameResource gameResource, int amount)
        {
            _storage[gameResource] -= amount;
            ResourceAmountChanged?.Invoke(gameResource, _storage[gameResource]);
            SaveResources();
        }

        private void SaveResources()
        {
            _resourceStorageProvider.UpdateStorage();
        }
    }
}