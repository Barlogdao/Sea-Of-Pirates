using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Interfaces.Stats;
using Project.Systems.Stats;

namespace Project.Systems.Stats
{
    public class PlayerStats : IPlayerStats, IUpgradableStats
    {
        private IPlayerStatsProvider _provider;
        private Dictionary<StatType, PlayerStat> _stats;

        public PlayerStats(IPlayerStatsProvider provider)
        {
            _provider = provider;
            _stats = _provider.LoadStats();

            UpdateStatsValues();
        }

        public int MaxHealth { get; private set; }
        public int Damage { get; private set; }
        public int CargoSize { get; private set; }
        public int Speed { get; private set; }
        public int AttackRange { get; private set; }
        public int CannonsAmount { get; private set; }

        public void UpgradeStat(StatType type)
        {
            _stats[type].LevelUp();
            SetStatValues();
        }

        public int GetStatValue(StatType type)
        {
            return _stats[type].GetValue();
        }

        public int GetStatLevel(StatType type)
        {
            return _stats[type].Level;
        }

        private void UpdateStatsValues()
        {
            SetStatValues();
            SaveStats();
        }

        private void SetStatValues()
        {
            MaxHealth = GetStatValue(StatType.Health);
            Damage = GetStatValue(StatType.Damage);
            CargoSize = GetStatValue(StatType.CargoSize);
            CannonsAmount = GetStatValue(StatType.CannonsAmount);
            Speed = GetStatValue(StatType.Speed);
            AttackRange = GetStatValue(StatType.AttackRange);
        }

        private void SaveStats()
        {
            _provider.UpdateStats();
        }
    }
}

public interface IUpgradableStats
{
    int GetStatLevel(StatType type);
    int GetStatValue(StatType type);
    void UpgradeStat(StatType type);
}