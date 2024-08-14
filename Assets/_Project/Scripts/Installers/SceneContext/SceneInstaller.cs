using Project.Enemies;
using Project.Interfaces.Hold;
using Project.Players.Hold;
using Project.Spawner;
using Project.Systems.Leaderboard;
using Project.Systems.Quests;
using Project.Systems.Shop;
using Project.Systems.Upgrades;
using Project.UI.Leaderboard;
using Project.UI.Quests;
using Project.UI.Reward;
using Project.UI.Shop;
using Project.UI.Upgrades;
using Zenject;

namespace Project.Installers.SceneContext
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindEnemies();
            BindSystems();
            BindPlayer();
        }
        private void BindEnemies()
        {
            Container.Bind<EnemySpawner>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesTo<EnemyDeathNotifier>().AsSingle();
        }

        private void BindSystems()
        {
            BindQuestSystem();
            BindShopSystem();
            BindUpgradeSystem();
            BindLeaderboardSystem();

            Container.Bind<RewardView>().FromComponentInHierarchy().AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<IPlayerHold>().To<PlayerHold>().AsSingle();
        }

        private void BindQuestSystem()
        {
            Container.Bind<QuestWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestButton>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestView>().AsSingle().NonLazy();

            Container.Bind<QuestGiver>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesTo<QuestSystem>().FromNew().AsSingle();
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