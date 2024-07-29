using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Systems.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeBar : MonoBehaviour
{
    [SerializeField] private Image _statIcon;

    [SerializeField] private TMP_Text _statName;
    [SerializeField] private TMP_Text _levelProgress;
    [SerializeField] private TMP_Text _currentStatValue;
    [SerializeField] private TMP_Text _nextStatValue;

    [SerializeField] private Button _upgradeButton;

    [SerializeField] private UpgradeCostView _upgradeCostViewPrefab;
    [SerializeField] private RectTransform _upgradePriceHolder;

    private readonly List<UpgradeCostView> _upgradePriceView = new();

    private StatConfig _config;
    private IUpgradableStats _stats;
    private IPlayerStorage _playerStorage;

    private StatType _statType;

    private StatType StatType => _statType;
    private int CurrentStatLevel => _stats.GetStatLevel(_statType);

    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(TryUpgradeStat);
        
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(TryUpgradeStat);
    }

    public void Initialize(StatConfig config, IUpgradableStats stats, IPlayerStorage playerStorage)
    {
        _config = config;
        _stats = stats;
        _playerStorage = playerStorage;
        _statIcon.sprite = _config.Sprite;
        _statType = _config.StatType;
        _statName.text = _config.Name;

        UpdateView();
    }

    private void UpdateView()
    {
        int currentLevel = _stats.GetStatLevel(StatType);
        int nextLevel = currentLevel + 1;
        int currentValue = _config.GetValue(currentLevel);
        int nextValue = _config.GetValue(nextLevel);

        _levelProgress.text = $"{currentLevel} / {_config.MaxLevel}";

        _currentStatValue.text = currentValue.ToString();
        _nextStatValue.text = nextValue.ToString();

        var upgradePrice = _config.GetUpgradePrice(currentLevel);
        UpdateButtonView(upgradePrice, currentLevel);
        
    }

    private void UpdateButtonView(List<GameResourceAmount> upgradePrice, int currentLevel)
    {
        if (_config.IsMaxLevel(currentLevel))
        {
            _upgradeButton.gameObject.SetActive(false);
            return;
        }

        UpdateUpgradePriceView(upgradePrice);
        _upgradeButton.interactable = _playerStorage.CanSpend(upgradePrice);
    }

    private void UpdateUpgradePriceView(List<GameResourceAmount> upgradePrice)
    {
        foreach (var upgradeCostView  in _upgradePriceView)
        {
            upgradeCostView.Hide();
        }

        for (int i = 0; i < upgradePrice.Count; i++)
        {
            if (_upgradePriceView.Count <= i)
            {
                UpgradeCostView upgradeCostView = Instantiate(_upgradeCostViewPrefab, _upgradePriceHolder);
                _upgradePriceView.Add(upgradeCostView);
            }

            GameResourceAmount upgradeCost = upgradePrice[i];
            _upgradePriceView[i].UpdateView(upgradeCost.Resource.Sprite, upgradeCost.Amount.ToString());
        }
    }

    private void TryUpgradeStat()
    {
        List<GameResourceAmount> upgradePrice = _config.GetUpgradePrice(CurrentStatLevel);

        if (_playerStorage.TrySpendResource(upgradePrice))
        {
            _stats.UpgradeStat(StatType);
            UpdateView();
        }
    }
}
