using UnityEngine;
using Zenject;

namespace KibbleSpace
{
    //Installer for Scene Context
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public GameObject Dog;
        public GameObject KibblePrefab;

        public override void InstallBindings()
        {
            Container.Bind<PoolManager>().AsSingle();
            Container.Bind<ILateDisposable>().To<PoolManager>().AsSingle();

            Container.Bind<DogfoodSizeManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<TextManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BeagleFacade>().FromComponentInHierarchy().AsSingle();
            Container.Bind<KibbleCreationManager>().FromComponentInHierarchy().AsSingle();

            Container.BindMemoryPool<KibbleFacade, KibbleFacade.Pool>()
                .WithInitialSize(20)
                .ExpandByOneAtATime()
                .FromComponentInNewPrefab(KibblePrefab)
                .UnderTransformGroup("Kibbles");

            Container.DeclareSignal<EatAndDespawnSignal>();
            Container.DeclareSignal<DeadDogSignal>();
            Container.DeclareSignal<RestartSignal>();
        }

    }
}