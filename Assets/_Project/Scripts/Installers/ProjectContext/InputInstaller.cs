using Project.Players.Inputs;
using Zenject;

namespace Project.Installers.ProjectContext
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (Agava.WebUtility.Device.IsMobile)
            {
                Container.Bind<IInputService>().To<MobileInputService>().FromNew().AsSingle();
            }
            else
            {
                Container.Bind<IInputService>().To<StandaloneInputService>().FromNew().AsSingle();
            }
        }
    }
}