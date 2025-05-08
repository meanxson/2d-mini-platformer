using Zenject;

namespace Client
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromComponentInNewPrefabResource("SceneLoader").AsSingle().NonLazy();
        }
    }
}