using Project.Systems.Stats;
using System;

namespace Project.Interfaces.Hold
{
    public interface IPlayerHold
    {
        event Action<int> CargoChanged;

        void AddResource(GameResourceAmount gameResourceAmount);

        void LoadToStorage();
    }
}