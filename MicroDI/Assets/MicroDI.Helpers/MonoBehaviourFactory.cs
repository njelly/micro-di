using Assets.MicroDI.Interfaces;
using MicroDI.Interfaces;
using UnityEngine;

namespace MicroDI.Helpers
{
    public class MonoBehaviourFactory : IFactory
    {
        private readonly ComponentService _componentService;

        public MonoBehaviourFactory(ComponentService componentService)
        {
            _componentService = componentService;
        }

        public object Create(IContainer container, IBinding binding, params object[] args)
        {
            GameObject targetGameObject = null;

            // GameObject is passed....
            if(args.Length > 0 && args[0] is GameObject)
            {
                targetGameObject = (GameObject)args[0];
            }
            else
            {
                // No GameObject passed...   
                targetGameObject = new GameObject(binding.BoundType.Name);
            }

            // Get or Add the component...
            var component = targetGameObject.GetComponent(binding.BoundType) ? targetGameObject.GetComponent(binding.BoundType) : targetGameObject.AddComponent(binding.BoundType);

            // Inject [Inject] && [PathInject] attributes...
            _componentService.Inject(component);

            // Check for singleton...
            if(binding.BindingType == Enum.BindingType.Singleton)
            {
                GameObject.DontDestroyOnLoad(targetGameObject);
            }

            return component;
        }
    }
}