using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Interfaces.Stats;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI.Upgrades
{
    [RequireComponent(typeof(Canvas))]
    public class UpgradeWindow : MonoBehaviour
    {
        [SerializeField] private StatUpgradeBar _barPrefab;
        [SerializeField] private RectTransform _barHolder;
        [SerializeField] private Button _closeButton;

        private readonly List<StatUpgradeBar> _bars = new();

        private Canvas _windowCanvas;
        private StatsSheet _statsSheet;
        private IUpgradableStats _stats;
        private IPlayerStorage _playerStorage;

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Hide);

            foreach (StatUpgradeBar bar in _bars)
                bar.StatUpgraded -= OnStatUpgraded;
        }

        [Inject]
        public void Construct(StatsSheet statsSheet, IUpgradableStats stats, IPlayerStorage playerStorage)
        {
            _statsSheet = statsSheet;
            _stats = stats;
            _playerStorage = playerStorage;
            _windowCanvas = GetComponent<Canvas>();
            _closeButton.onClick.AddListener(Hide);

            CreateUpgradeBars();
            Hide();
        }

        public void Show()
        {
            _windowCanvas.enabled = true;

            foreach (StatUpgradeBar bar in _bars)
            {
                bar.Fill();
            }
        }

        public void Hide()
        {
            _windowCanvas.enabled = false;
        }

        private void CreateUpgradeBars()
        {
            foreach (StatConfig statConfig in _statsSheet.Stats)
            {
                StatUpgradeBar upgradeBar = Instantiate(_barPrefab, _barHolder);
                upgradeBar.Initialize(statConfig, _stats, _playerStorage);
                upgradeBar.StatUpgraded += OnStatUpgraded;

                _bars.Add(upgradeBar);
            }
        }

        private void OnStatUpgraded()
        {
            foreach (StatUpgradeBar bar in _bars)
            {
                bar.CheckUpgradePrice();
            }
        }
    }
}