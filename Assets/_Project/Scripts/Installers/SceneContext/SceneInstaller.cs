using Cinemachine;
using Project.Configs.Level;
using Project.Controllers;
using Project.Enemies;
using Project.Enemies.Logic;
using Project.Interactables;
using Project.Players.Logic;
using Project.Players.View;
using Project.Spawner;
using Project.Systems.Cameras;
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
using SimpleInputNamespace;
using UnityEngine;
using Zenject;

namespace Project.Installers.SceneContext
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private bool _isMobile = false;

        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private VfxSpawner _vfxSpawner;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private JoystickCanvas _joystickCanvas;

        public LevelConfig LevelConfig => _levelConfig;

        public override void InstallBindings()
        {
            BindConfigs();
            BindSpawners();
            BindUI();
            BindSystems();
            BindInteractables();
            BindPlayer();
        }


        private void BindConfigs()
        {
            Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
        }

        private void BindSpawners()
        {
            Container.Bind<VfxSpawner>().FromComponentInNewPrefab(_vfxSpawner).AsSingle();
            Container.Bind<EnemyFactory>().FromNew().AsSingle().WithArguments(_enemyPrefab);
            Container.Bind<BaseEnemySpawner>().FromComponentsInHierarchy().AsCached();
            Container.Bind<ShopItemFactory>().AsSingle();
        }

        private void BindSystems()
        {
            Container.Bind<CinemachineBrain>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Joystick>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<QuestSystem>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ShopSystem>().FromNew().AsSingle().NonLazy();
            Container.Bind<UpgradeSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LeaderboardSystem>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<QuestEnemyHandler>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesTo<EnemyDeathNotifier>().AsSingle();
            Container.Bind<CameraSystem>().FromComponentInHierarchy().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<QuestWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestButton>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestView>().AsSingle().NonLazy();

            Container.Bind<ShopWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShopButton>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UpgradeWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeButton>().FromComponentInHierarchy().AsSingle();

            Container.Bind<LeaderboardWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LeaderboardButton>().FromComponentInHierarchy().AsSingle();

            Container.Bind<RewardView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<NextLevelWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerDeathWindow>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UiCanvas>().FromComponentInHierarchy().AsSingle();
        }


        private void BindInteractables()
        {
            Container.Bind<QuestGiver>().FromComponentsInHierarchy().AsCached();
            Container.Bind<PirateBay>().FromComponentInHierarchy().AsSingle();
        }

        private void BindPlayer()
        {
            BindMoveHandler();

            Container.BindInterfacesTo<PlayerHold>().AsSingle();
            Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerAttack>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<PlayerLootController>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerSpawner>().FromNew().AsSingle().NonLazy();
        }

        private void BindMoveHandler()
        {
            if (_isMobile)
            {
                Container.Bind<JoystickCanvas>().FromComponentInNewPrefab(_joystickCanvas).AsSingle();
                Container.Bind<MoveHandler>().To<MobileMoveHandler>().FromNew().AsSingle();
            }
            else
            {
                Container.Bind<MoveHandler>().To<DesktopMoveHandler>().FromNew().AsSingle();
            }

            //if (Agava.WebUtility.Device.IsMobile)
            //{
            //    Container.Bind<JoystickCanvas>().FromComponentInNewPrefab(_joystickCanvas).AsSingle();
            //    Container.Bind<MoveHandler>().To<MobileMoveHandler>().FromNew().AsSingle();
            //}
            //else
            //{
            //    Container.Bind<MoveHandler>().To<DesktopMoveHandler>().FromNew().AsSingle();
            //}
        }
    }
}