using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Systems.Stats;

namespace Project.Players.ResourcesHold
{
    public class PlayerHold : IPlayerHold
    {
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly List<GameResourceAmount> _cargo;

        public PlayerHold(IPlayerStatsProvider playerStatsProvider)
        {
            _playerStatsProvider = playerStatsProvider;
            CargoSize = _playerStatsProvider.LoadStats()[StatType.CargoSize].GetValue();
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

        public List<GameResourceAmount> TakeResources()
        {
            var cargoCopy = _cargo;
            _cargo.Clear();

            return cargoCopy;
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