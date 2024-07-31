using System.Collections.Generic;
using Project.Systems.Stats;

namespace Project.Players.ResourcesHold
{
    public class PlayerHold
    {
        private int _cargoSize;
        private List<GameResourceAmount> _cargo;

        public PlayerHold(int cargoSize)
        {
            _cargoSize = cargoSize;
            _cargo = new List<GameResourceAmount>();
        }

        public void AddResource(GameResourceAmount gameResourceAmount)
        {
            if (_cargo.Count < _cargoSize)
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

        public void UpgradeHold()
        {
            _cargoSize++;
        }

        public int GetResourcesAmount()
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