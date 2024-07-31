using Project.Systems.Stats;

namespace Project.Players.Hold
{
    public interface IPlayerHold
    {
        void AddResource(GameResourceAmount gameResourceAmount);
        void TransferResources();
    }
}