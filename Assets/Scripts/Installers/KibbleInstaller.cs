using Zenject;

namespace KibbleSpace
{
    //Installer for the Game Context within a Kibble
    public class KibbleInstaller : MonoInstaller<KibbleInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KibbleMovementHandler>().AsSingle();
            Container.Bind<KibbleFacade>().AsSingle();
        }
    }
}
