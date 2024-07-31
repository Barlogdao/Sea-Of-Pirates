using Project.Enemies;
using Project.Interfaces.Hold;
using Project.Players.Hold;
using Project.Spawner;
using Project.Systems.Quests;
using Project.UI.Quests;
using Project.UI.Reward;
using Project.UI.Upgrades;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Installers.SceneContext
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindEnemies();
            BindUI();
            BindQuestSystem();
            BindPlayer();
        }

        private void BindEnemies()
        {
            Container.Bind<EnemySpawner>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesTo<EnemyDeathNotifier>().AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<QuestWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestButton>().FromComponentInHierarchy().AsSingle();
            Container.Bind<QuestView>().AsSingle().NonLazy();

            Container.Bind<UpgradeWindow>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeButton>().FromComponentInHierarchy().AsSingle();
            Container.Bind<UpgradeSystemView>().AsSingle().NonLazy();

            Container.Bind<RewardView>().FromComponentInHierarchy().AsSingle();
        }

        private void BindQuestSystem()
        {
            Container.Bind<QuestGiver>().FromComponentsInHierarchy().AsCached();
            Container.BindInterfacesTo<QuestSystem>().FromNew().AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<IPlayerHold>().To<PlayerHold>().AsSingle();
        }
    }
}