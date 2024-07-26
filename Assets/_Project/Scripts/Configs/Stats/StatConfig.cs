using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Systems.Stats;
using Project.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "StatConfig", menuName = "Configs/Stats/StatConfig")]

public class StatConfig : ScriptableObject
{
    [SerializeField] private UpgradeCost _primaryCost;
    [SerializeField] private AdditionalUpgradeCost _secondaryCost;

    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField, Min(0)] public int MinValue { get; private set; }
    [field: SerializeField, Min(0)] public int MaxValue { get; private set; }
    [field: SerializeField, Min(0)] public int MaxLevel { get; private set; }

    public int MinLevel => 0;

    public int GetValue(int currentLevel)
    {
        return (int)ExtendedMath.Remap(currentLevel, MinLevel, MaxLevel, MinValue, MaxValue);
    }

    public List<GameResourceAmount> GetUpgradePrice(int currentLevel)
    {
        int nextLevel = currentLevel + 1;

        List<GameResourceAmount> price = new()
        {
            _primaryCost.GetCost(this, nextLevel)
        };

        if (_secondaryCost.IsApplicable(nextLevel))
        {
            price.Add(_secondaryCost.GetCost(this, nextLevel));
        }

        return price;
    }

    public bool IsMaxLevel(int currentLevel)
    {
        return currentLevel == MaxLevel;
    }
}

public class UpgradeSystem
{
    private readonly StatsSheet _statsSheet;
    private readonly IUpgradableStats _stats;
    private readonly IPlayerStorage _playerStorage;

    public UpgradeSystem(StatsSheet statsSheet, IUpgradableStats stats, IPlayerStorage playerStorage)
    {
        _statsSheet = statsSheet;
        _stats = stats;
        _playerStorage = playerStorage;
    }

}