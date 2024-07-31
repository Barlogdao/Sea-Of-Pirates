using Project.Systems.Stats;

namespace Project.Interfaces.Stats
{
    public interface IUpgradableStats
    {
        int GetStatLevel(StatType type);
        int GetStatValue(StatType type);
        void UpgradeStat(StatType type);
    }
}