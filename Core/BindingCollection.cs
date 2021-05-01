using MicroDI.Interfaces;
using System;
using System.Collections.Generic;

namespace MicroDI.Core
{
    public class BindingCollection
    {
        private Dictionary<Type, List<IBinding>> _bindings;
        private Dictionary<Type, Dictionary<object, IBinding>> _cachedTagLookup;

        public BindingCollection()
        {
            _bindings = new Dictionary<Type, List<IBinding>>();
            _cachedTagLookup = new Dictionary<Type, Dictionary<object, IBinding>>();
        }

        public IBinding Bind(Type keyType, Type bindType)
        {
            // Reset the cache for type...
            _cachedTagLookup.Remove(keyType);

            if(!_bindings.ContainsKey(keyType))
            {
                _bindings[keyType] = new List<IBinding>();
            }

            var binding = new Binding
            {
                KeyType = keyType,
                BoundType = bindType
            };

            _bindings[keyType].Add(binding);

            return binding;
        }

        private void PreCacheBindingsForType(Type keyType)
        {
            if(!_bindings.ContainsKey(keyType))
            {
                return;
            }

            if(!_cachedTagLookup.ContainsKey(keyType))
            {
                _cachedTagLookup.Add(keyType, new Dictionary<object, IBinding>());
            }

            if(_cachedTagLookup.ContainsKey(keyType))
            {
                foreach(var binding in _bindings[keyType])
                {
                    _cachedTagLookup[keyType][binding.Tag] = binding;
                }
            }
        }

        private IBinding GetCachedBinding(Type keyType, object tag)
        {
            if(_cachedTagLookup.ContainsKey(keyType))
            {
                if(_cachedTagLookup[keyType].ContainsKey(tag))
                {
                    return _cachedTagLookup[keyType][tag];
                }
            }

            return null;
        }

        public IBinding GetBinding(Type keyType, object tag)
        {
            tag = tag ?? TagUtility.GetNullTag();

            PreCacheBindingsForType(keyType);
            return GetCachedBinding(keyType, tag);
        }

        public bool HasBinding(Type keyType, object tag)
        {
            tag = tag ?? TagUtility.GetNullTag();

            PreCacheBindingsForType(keyType);
            return (_cachedTagLookup.ContainsKey(keyType) && _cachedTagLookup[keyType].ContainsKey(tag));
        }

        public void Dispose()
        {
            foreach(var bindings in _bindings.Values)
            {
                foreach (var binding in bindings)
                {
                    binding.Dispose();
                }
            }

            _bindings = null;
            _cachedTagLookup = null;
        }
    }
}
