using Project.Interfaces.Data;
using UnityEngine;
using Zenject;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private StatUpgradeBar _barPrefab;
    [SerializeField] private RectTransform _barHolder;

    private StatsSheet _statsSheet;
    private  IUpgradableStats _stats;
    private  IPlayerStorage _playerStorage;

    [Inject]
    public void Construct(StatsSheet statsSheet, IUpgradableStats stats, IPlayerStorage playerStorage)
    {
        _statsSheet = statsSheet;
        _stats = stats;
        _playerStorage = playerStorage;

        foreach (StatConfig stat in _statsSheet.Stats)
        {
            StatUpgradeBar upgradeBar = Instantiate(_barPrefab, _barHolder);
            upgradeBar.Initialize(stat, _stats, _playerStorage);
        }
    }
}
