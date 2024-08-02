using Project.Configs.GameResources;
using Project.Interfaces.Data;
using Project.Systems.Data;
using Project.Systems.Stats;
using Project.Systems.Storage;
using UnityEngine;
using Zenject;

namespace Project.Installers.ProjectContext
{
    public class GameDataInstaller : MonoInstaller
    {
        [SerializeField] private GameResourcesSheet _resourcesSheet;
        [SerializeField] private StatsSheet _statsSheet;

        public override void InstallBindings()
        {
            Container.Bind<GameResourcesSheet>().FromInstance(_resourcesSheet);
            Container.Bind<StatsSheet>().FromInstance(_statsSheet);

            Container.BindInterfacesAndSelfTo<GameDataService>().AsSingle().NonLazy();

            Container.Bind<IResourceStorageProvider>().To<ResourceStorageProvider>().AsSingle();
            Container.Bind<IPlayerStatsProvider>().To<PlayerStatsProvider>().AsSingle();
            Container.Bind<IQuestsProvider>().To<QuestsProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerStats>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStorage>().AsSingle();
        }
    }
}