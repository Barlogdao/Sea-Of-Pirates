using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Interfaces.Stats;
using Project.Systems.Stats;

namespace Project.Players.ResourcesHold
{
    public class PlayerHold : IPlayerHold
    {
        private readonly IPlayerStats _playerStats;
        private readonly IPlayerStorage _playerStorage;
        private readonly List<GameResourceAmount> _cargo;

        public PlayerHold(IPlayerStats playerStats, IPlayerStorage playerStorage)
        {
            CargoSize = playerStats.CargoSize;
            _playerStorage = playerStorage;
            _cargo = new List<GameResourceAmount>();
        }
        
        public int CargoSize { get; private set; }

        public void AddResource(GameResourceAmount gameResourceAmount)
        {
            if (GetResourcesAmount() < CargoSize)
            {
                _cargo.Add(gameResourceAmount);
            }
        }

        public void TransferResources()
        {
            _playerStorage.AddResource(_cargo);
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