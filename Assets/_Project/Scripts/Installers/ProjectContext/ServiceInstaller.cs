using Lean.Localization;
using Project.Interfaces.SDK;
using Project.SDK.Advertisment;
using Project.SDK.InApp;
using Project.SDK.Leaderboard;
using Project.Systems.Audio;
using Project.Systems.Pause;
using System;
using UnityEngine;
using Zenject;

namespace Project.Installers.ProjectContext
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioService _audioServicePrefab;
        [SerializeField] private LeanLocalization _localizationPrefab;

        public override void InstallBindings()
        {
            Container.Bind<LeanLocalization>().FromComponentInNewPrefab(_localizationPrefab).AsSingle().NonLazy();

            Container.Bind<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle();
            Container.Bind<PauseService>().FromNew().AsSingle();
            Container.Bind<AdvertismentController>().AsSingle();
            Container.Bind<AdvertismentService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<FocusController>().FromNew().AsSingle().NonLazy();

            BindSDK();
        }

        private void BindSDK()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Container.Bind<IBillingProvider>().To<BillingProvider>().FromNew().AsSingle();
            Container.Bind<ILeaderboardService>().To<YandexLeaderboardService>().FromNew().AsSingle();
#else
            Container.Bind<IBillingProvider>().To<MockBillingProvider>().FromNew().AsSingle();
            Container.Bind<ILeaderboardService>().To<MockLeaderboardService>().FromNew().AsSingle();
#endif
        }
    }
}