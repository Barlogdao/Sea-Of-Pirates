using Project.Configs.Level;
using Project.Enemies;
using Project.Enemies.Logic;
using Project.Interactables;
using Project.Players.Logic;
using Project.Players.View;
using Project.Spawner;
using Project.Systems.Leaderboard;
using Project.Systems.Quests;
using Project.Systems.Shop;
using Project.Systems.Upgrades;
using Project.UI;
using Project.UI.Leaderboard;
using Project.UI.Quests;
using Project.UI.Reward;
using Project.UI.Shop;
using Project.UI.Upgrades;
using UnityEngine;
using Zenject;

namespace Project.Installers.SceneContext
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private VfxSpawner _vfxSpawner;
        [SerializeField] private Enemy _enemyPrefab;

        public override void InstallBindings()
        {
            BindConfigs();
            BindEnemies();
            BindSystems();
            BindUI();
            BindInteractables();
            BindPlayer();
        }


        private void BindConfigs()
        {
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
            Container.Bind<VfxSpawner>().FromInstance(_vfxSpawner).AsSingle();
        }

        private void BindEnemies()
        {
            Container.Bind<EnemyFactory>().FromNew().AsSingle().WithArguments(_enemyPrefab);
            Container.Bind<EnemySpawner>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesTo<EnemyDeathNotifier>().AsSingle();
        }

        private void BindSystems()
        {
            BindQuestSystem();
            BindShopSystem();
            BindUpgradeSystem();
            BindLeaderboardSystem();
        }

        private void BindUI()
        {
            Container.Bind<RewardView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NextLevelWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerDeathWindow>().FromComponentInHierarchy().AsSingle();
        }


        private void BindInteractables()
        {
            Container.Bind<PirateBay>().FromComponentInHierarchy().AsSingle();
        }

        private void BindPlayer()
        {
            Container.BindInterfacesTo<PlayerHold>().AsSingle();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerAttack>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<PlayerLootController>().FromNew().AsSingle().NonLazy();

        }

        private void BindQuestSystem()
        {
            Container.Bind<QuestWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestButton>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestView>().AsSingle().NonLazy();

            Container.Bind<QuestGiver>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesAndSelfTo<QuestSystem>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<QuestEnemyMarker>().FromNew().AsSingle().NonLazy();
        }

        private void BindShopSystem()
        {
            Container.Bind<ShopWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShopButton>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ShopItemFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopSystem>().FromNew().AsSingle().NonLazy();
        }

        private void BindUpgradeSystem()
        {
            Container.Bind<UpgradeWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeButton>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UpgradeSystem>().AsSingle().NonLazy();
        }

        private void BindLeaderboardSystem()
        {
            Container.Bind<LeaderboardWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LeaderboardButton>().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<LeaderboardSystem>().FromNew().AsSingle().NonLazy();
        }
    }
}