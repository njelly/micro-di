using MicroDI.Interfaces;
using System;

namespace MicroDI.Core
{
    public class Container : IContainer
    {
        private BindingCollection _bindings;

        public IInjector Injector { get; private set; }

        public Container()
        {
            _bindings = new BindingCollection();
            Injector = new Injector(this);
        }

        public IBinding Bind<TKey, TBind>() where TBind : TKey
        {
            return Bind(typeof(TKey), typeof(TBind));
        }

        public IBinding Bind(Type keyType, Type boundType)
        {
            return _bindings.Bind(keyType, boundType);
        }

        public IBinding Bind<TKey>()
        {
            return Bind(typeof(TKey), typeof(TKey));
        }

        public void Bootstrap<TBootstrap>() where TBootstrap : IBootstrap
        {
            var bootstrap = Activator.CreateInstance<TBootstrap>();
            bootstrap.SetupBindings(this);
        }

        public void Bootstrap(Type type)
        {
            if(!typeof(IBootstrap).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Specified Bootstrap Type {type} does not inherit from IBoostrap!");
            }

            var bootstrap = (IBootstrap)Activator.CreateInstance(type);
            bootstrap.SetupBindings(this);
        }

        public void Dispose()
        {
            _bindings.Dispose();
        }

        public IBinding GetBinding<TKey>(object tag = null)
        {
            return GetBinding(typeof(TKey), tag);
        }

        public IBinding GetBinding(Type keyType, object tag = null)
        {
            var binding = _bindings.GetBinding(keyType, tag);

            return binding;
        }

        public bool HasBinding<TKey>(object tag = null)
        {
            return HasBinding(typeof(TKey), tag);
        }

        public bool HasBinding(Type keyType, object tag = null)
        {
            var hasBinding = _bindings.HasBinding(keyType, tag);

            return hasBinding;
        }

        public TKey Resolve<TKey>(object tag = null)
        {
            return (TKey)Resolve(typeof(TKey), tag);
        }

        public object Resolve(Type keyType, object tag = null)
        {
            return ResolveWith(keyType, tag);
        }

        public TKey ResolveWith<TKey>(object tag = null, params object[] args)
        {
            return (TKey)ResolveWith(typeof(TKey), tag, args);
        }

        public object ResolveWith(Type keyType, object tag = null, params object[] args)
        {
            // Check Locals...
            var binding = GetBinding(keyType, tag);

            if (binding != null)
            {
                var instance = binding.Instance ?? Injector.CreateInstance(binding, args);

                if (binding.BindingType == Enum.BindingType.Singleton)
                {
                    binding.Instance = instance;
                }

                return instance;
            }

            return null;
        }
    }
}
