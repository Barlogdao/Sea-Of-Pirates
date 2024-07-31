using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Interfaces.Stats;
using Project.Systems.Stats;

namespace Project.Players.Hold
{
    public class PlayerHold : IPlayerHold
    {
        private readonly IPlayerStats _playerStats;
        private readonly IPlayerStorage _playerStorage;
        private readonly List<GameResourceAmount> _cargo;

        private int _cargoSize => _playerStats.CargoSize;

        public PlayerHold(IPlayerStats playerStats, IPlayerStorage playerStorage)
        {
            _playerStats = playerStats;
            _playerStorage = playerStorage;
            _cargo = new List<GameResourceAmount>();
        }

        public void AddResource(GameResourceAmount gameResourceAmount)
        {
            if (GetResourcesAmount() < _cargoSize)
            {
                _cargo.Add(gameResourceAmount);
            }
        }

        public void TransferResources()
        {
            _playerStorage.AddResource(_cargo);
            _cargo.Clear();
        }

        private int GetResourcesAmount()
        {
            int resourcesAmount = 0;

            for (int i = 0; i < _cargo.Count; i++)
            {
                resourcesAmount += _cargo[i].Amount;
            }

            return resourcesAmount;
        }
    }
}