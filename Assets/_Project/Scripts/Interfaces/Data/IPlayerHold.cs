using System.Collections.Generic;
using Project.Systems.Stats;

namespace Project.Interfaces.Data
{
    public interface IPlayerHold
    {
        void AddResource(GameResourceAmount gameResourceAmount);
        void TransferResources();
    }
}