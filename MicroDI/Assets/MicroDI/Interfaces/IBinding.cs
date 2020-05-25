using Assets.MicroDI.Interfaces;
using MicroDI.Enum;
using System;

namespace MicroDI.Interfaces
{
    public delegate object ConstructorHandler(IContainer container, IBinding binding, params object[] args);

    public interface IBinding
    {
        Type KeyType { get; set; }
        Type BoundType { get; set; }
        BindingType BindingType { get; set; }
        ConstructorHandler Factory { get; set; }
        Type FactoryType { get; set; }
        object Tag { get; set; }
        object Instance { get; set; }

        IBinding AsSingleton();
        IBinding WithFactory(ConstructorHandler constructorHandler);
        IBinding WithFactoryType<TFactory>() where TFactory : IFactory;
        IBinding WithTag(object tag);
        void Dispose();
    }
}
