using Project.Players.Inputs;
using Project.Systems.Data;
using Project.Systems.Storage;
using Zenject;

namespace Project.Installers.ProjectContext
{
    public class GameSystemsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerStats>().AsSingle();
            Container.BindInterfacesTo<PlayerStorage>().AsSingle();

            BindInput();
        }

        private void BindInput()
        {
            Container.Bind<IInputService>().To<InputService>().FromNew().AsSingle();
        }
    }
}