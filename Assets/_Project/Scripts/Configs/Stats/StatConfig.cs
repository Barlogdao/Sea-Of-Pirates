using System.Collections.Generic;
using Lean.Localization;
using Project.Systems.Stats;
using Project.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "StatConfig", menuName = "Configs/Stats/StatConfig")]

public class StatConfig : ScriptableObject
{
    [SerializeField] private UpgradeCost _primaryCost;
    [SerializeField] private AdditionalUpgradeCost _secondaryCost;

    [SerializeField, LeanTranslationName] private string _nameToken;
    [SerializeField, LeanTranslationName] private string _descriptionToken;

    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField, Min(0)] public int MinValue { get; private set; }
    [field: SerializeField, Min(0)] public int MaxValue { get; private set; }
    [field: SerializeField, Min(0)] public int MaxLevel { get; private set; }
    public string Name => LeanLocalization.GetTranslationText(_nameToken);
    public string Description => LeanLocalization.GetTranslationText(_descriptionToken);

    public int MinLevel => 0;

    public int GetValue(int level)
    {
        return (int)ExtendedMath.Remap(level, MinLevel, MaxLevel, MinValue, MaxValue);
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
