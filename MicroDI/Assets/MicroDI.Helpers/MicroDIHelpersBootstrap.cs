using MicroDI.Interfaces;

namespace MicroDI.Helpers
{
    public class MicroDIHelpersBootstrap : IBootstrap
    {
        public void SetupBindings(IContainer container)
        {
            container.Bind<MonoBehaviourFactory>().AsSingleton();
            container.Bind<ComponentService>().AsSingleton();
            container.Bind<UpdateService>().AsSingleton().WithFactoryType<MonoBehaviourFactory>();
            container.Bind<GameObjectFactory>().AsSingleton();
        }
    }
}
