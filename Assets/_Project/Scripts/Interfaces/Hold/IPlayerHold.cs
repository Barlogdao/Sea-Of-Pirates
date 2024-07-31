using Project.Systems.Stats;

namespace Project.Interfaces.Hold
{
    public interface IPlayerHold
    {
        void AddResource(GameResourceAmount gameResourceAmount);
        void TransferResources();
    }
}