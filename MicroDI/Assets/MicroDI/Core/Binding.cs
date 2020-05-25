using Assets.MicroDI.Interfaces;
using MicroDI.Enum;
using MicroDI.Interfaces;
using System;

namespace MicroDI.Core
{
    public class Binding : IBinding, IEquatable<Binding>
    {
        public Type KeyType { get; set; }
        public Type BoundType { get; set; }
        public BindingType BindingType { get; set; }
        public object Instance { get; set; }
        public ConstructorHandler Factory { get; set; }
        public Type FactoryType { get; set; }
        public object Tag { get; set; }

        public Binding()
        {
            Tag = TagUtility.GetNullTag();
        }

        public IBinding AsSingleton()
        {
            BindingType = BindingType.Singleton;

            return this;
        }

        public void Dispose()
        {
            if(Instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Instance = null;
        }

        public bool Equals(Binding other)
        {
            return KeyType == other.KeyType && BoundType == other.BoundType && Tag == other.Tag;
        }

        public override bool Equals(object obj)
        {
            if(obj is IBinding other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return KeyType.GetHashCode() + BoundType.GetHashCode() + Tag.GetHashCode();
        }

        public IBinding WithFactory(ConstructorHandler constructorHandler)
        {
            Factory = constructorHandler;

            return this;
        }

        public IBinding WithFactoryType<TFactory>() where TFactory : IFactory
        {
            FactoryType = typeof(TFactory);

            return this;
        }

        public IBinding WithTag(object tag)
        {
            Tag = tag ?? TagUtility.GetNullTag();

            return this;
        }
    }
}
