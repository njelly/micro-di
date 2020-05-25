using Assets.MicroDI.Interfaces;
using MicroDI.Exceptions;
using MicroDI.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MicroDI.Core
{
    public class Injector : IInjector
    {
        private IContainer _container;
        private List<Type> _activeResolving;

        public Injector(IContainer container)
        {
            _container = container;
            _activeResolving = new List<Type>();
        }

        public object CreateInstance(IBinding binding, object[] args)
        {
            _activeResolving.Add(binding.KeyType);
            object instance = null;

            // Factory...
            if (binding.Factory != null)
            {
                instance = CreateWithCustomConstructor(binding, args);
            }

            // FactoryType...
            if(instance == null && binding.FactoryType != null)
            {
                instance = CreateWithFactoryType(binding, args);
            }

            // No factory, use constructor fallback...
            if(instance == null)
            {
                instance = CreateWithConstructor(binding, args);
            }
            
            _activeResolving.Remove(binding.KeyType);

            InjectMembers(instance);

            return instance;
        }

        private object CreateWithConstructor(IBinding binding, object[] args)
        {
            var constructor = ReflectionUtility.GetConstructor(binding.BoundType);
            var arguments = new List<object>(args);
            var expectedParameters = constructor.GetParameters();
            var parameters = new List<object>();

            foreach (var paramInfo in expectedParameters)
            {
                // IContainer...
                if(paramInfo.ParameterType == typeof(IContainer))
                {
                    parameters.Add(_container);
                    continue;
                }

                if (_container.HasBinding(paramInfo.ParameterType))
                {
                    if (paramInfo.ParameterType == binding.BoundType)
                    {
                        throw new RecursiveInjectionException($"Cannot recursively inject type {binding.BoundType}");
                    }

                    if (_activeResolving.Contains(paramInfo.ParameterType))
                    {
                        throw new CircularInjectionException($"Cannot circular resolve properties while injecting property: {paramInfo.ParameterType} {paramInfo.Name} in constructor of type: {binding.BoundType}");
                    }

                    parameters.Add(_container.Resolve(paramInfo.ParameterType));
                }
                else
                {
                    // Inject additional arguments if match...
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        if (paramInfo.ParameterType.IsAssignableFrom(arguments[i].GetType()))
                        {
                            parameters.Add(arguments[i]);
                            arguments.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // Fill out remaining...
            while (parameters.Count < expectedParameters.Length)
            {
                parameters.Add(null);
            }

            var instance = constructor.Invoke(parameters.ToArray());
            return instance;
        }

        private object CreateWithFactoryType(IBinding binding, object[] args)
        {
            var factory = (IFactory)_container.Resolve(binding.FactoryType);
            var instance = factory?.Create(_container, binding, args);

            if (!binding.BoundType.IsAssignableFrom(instance.GetType()))
            {
                throw new CustomFactoryInjectionException($"Can not cast type {instance.GetType()} to {binding.BoundType} while using custom factory for resolving {binding.KeyType}! Ensure you are creating the expected type.");
            }

            return instance;
        }

        private object CreateWithCustomConstructor(IBinding binding, object[] args)
        {
            var instance = binding.Factory.Invoke(_container, binding, args);

            if (!binding.BoundType.IsAssignableFrom(instance.GetType()))
            {
                throw new CustomFactoryInjectionException($"Can not cast type {instance.GetType()} to {binding.BoundType} while using custom factory for resolving {binding.KeyType}! Ensure you are creating the expected type.");
            }

            return instance;
        }

        public void InjectMembers(object target)
        {
            foreach(var memberInfo in ReflectionUtility.GetMembers(target.GetType()))
            {
                if(memberInfo is PropertyInfo propertyInfo)
                {
                    // IContainer...
                    if(propertyInfo.PropertyType == typeof(IContainer))
                    {
                        propertyInfo.SetValue(target, _container, null);
                        continue;
                    }

                    if(_container.HasBinding(propertyInfo.PropertyType))
                    {
                        var attr = ReflectionUtility.GetInjectAttribute(memberInfo);
                        var value = _container.Resolve(propertyInfo.PropertyType, attr.Tag);
                        propertyInfo.SetValue(target, value, null);
                    }
                }

                if(memberInfo is FieldInfo fieldInfo)
                {
                    // IContainer...
                    if(fieldInfo.FieldType == typeof(IContainer))
                    {
                        fieldInfo.SetValue(target, _container);
                    }

                    if(_container.HasBinding(fieldInfo.FieldType))
                    {
                        var attr = ReflectionUtility.GetInjectAttribute(memberInfo);
                        var value = _container.Resolve(fieldInfo.FieldType, attr.Tag);
                        fieldInfo.SetValue(target, value);
                    }
                }
            }
        }
    }
}
