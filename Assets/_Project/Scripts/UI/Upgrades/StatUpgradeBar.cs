using System;
using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Systems.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Upgrades
{
    public class StatUpgradeBar : MonoBehaviour
    {
        private const string LevelToken = "Уровень";

        [SerializeField] private Image _statIcon;

        [SerializeField] private TMP_Text _statName;
        [SerializeField] private TMP_Text _statDescription;
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

        public event Action StatUpgraded;

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
            _statDescription.text = _config.Description;
        }

        public void Fill()
        {
            int nextLevel = CurrentStatLevel + 1;
            
            SetStatValues(CurrentStatLevel, nextLevel);
            SetLevelProgress(CurrentStatLevel);
            CheckUpgradePrice();
        }

        private void SetLevelProgress(int currentLevel)
        {
            _levelProgress.text = $"{LevelToken}: {currentLevel} / {_config.MaxLevel}";
        }

        private void SetStatValues(int currentLevel, int nextLevel)
        {
            int currentValue = _config.GetValue(currentLevel);
            int nextValue = _config.GetValue(nextLevel);

            _currentStatValue.text = currentValue.ToString();
            _nextStatValue.text = nextValue.ToString();
        }

        public void CheckUpgradePrice()
        {
            int currentLevel = _stats.GetStatLevel(_statType);
            List<GameResourceAmount> upgradePrice = _config.GetUpgradePrice(currentLevel);

            UpdatePriceView(upgradePrice, currentLevel);
        }

        private void UpdatePriceView(List<GameResourceAmount> upgradePrice, int currentLevel)
        {
            if (_config.IsMaxLevel(currentLevel))
            {
                _upgradeButton.gameObject.SetActive(false);
                _nextStatValue.gameObject.SetActive(false);
                return;
            }

            SetUpgradePrice(upgradePrice);
            _upgradeButton.interactable = _playerStorage.CanSpend(upgradePrice);
        }

        private void SetUpgradePrice(List<GameResourceAmount> upgradePrice)
        {
            foreach (var upgradeCostView in _upgradePriceView)
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

                bool canSpend = _playerStorage.CanSpend(upgradeCost);
                _upgradePriceView[i].Set(upgradeCost.Resource.Sprite, upgradeCost.Amount.ToString(), canSpend);
            }
        }

        private void TryUpgradeStat()
        {
            List<GameResourceAmount> upgradePrice = _config.GetUpgradePrice(CurrentStatLevel);

            if (_playerStorage.TrySpendResource(upgradePrice))
            {
                _stats.UpgradeStat(_statType);
                Fill();

                StatUpgraded?.Invoke();
            }
        }
    }
}