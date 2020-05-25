using System;

namespace MicroDI.Interfaces
{
    public interface IContainer
    {
        IInjector Injector { get; }

        IBinding Bind<TKey, TBind>() where TBind : TKey;
        IBinding Bind<TKey>();
        IBinding Bind(Type keyType, Type boundType);

        TKey Resolve<TKey>(object tag = null);
        object Resolve(Type keyType, object tag = null);

        TKey ResolveWith<TKey>(object tag = null, params object[] args);
        object ResolveWith(Type keyType, object tag = null, params object[] args);

        IBinding GetBinding<TKey>(object tag = null);
        IBinding GetBinding(Type keyType, object tag = null);

        bool HasBinding<TKey>(object tag = null);
        bool HasBinding(Type keyType, object tag = null);

        void Bootstrap<TBootstrap>() where TBootstrap : IBootstrap;

        void Dispose();
    }
}
