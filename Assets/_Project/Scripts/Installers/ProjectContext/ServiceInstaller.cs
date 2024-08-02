using Project.SDK.Advertisment;
using Project.Systems.Audio;
using Project.Systems.Pause;
using UnityEngine;
using Zenject;

namespace Project.Installers.ProjectContext
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioService _audioServicePrefab;

        public override void InstallBindings()
        {
            Container.Bind<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle();
            Container.Bind<PauseService>().FromNew().AsSingle();
            Container.Bind<AdvertismentService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<FocusController>().FromNew().AsSingle().NonLazy();
        }
    }
}