using UnityEngine;

namespace MicroDI.Helpers
{
    public class GameObjectFactory
    {
        private readonly ComponentService _componentService;

        public GameObjectFactory(ComponentService componentService)
        {
            _componentService = componentService;
        }

        public GameObject Instantiate(GameObject original)
        {
            var instance = Object.Instantiate(original);
            InjectComponents(instance);

            return instance;
        }

        public GameObject Instantiate(GameObject original, Transform parent)
        {
            var instance = Object.Instantiate(original, parent);
            InjectComponents(instance);

            return instance;
        }

        public GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace)
        {
            var instance = Object.Instantiate(original, parent, instantiateInWorldSpace);
            InjectComponents(instance);

            return instance;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            var instance = Object.Instantiate(original, position, rotation);
            InjectComponents(instance);

            return instance;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var instance = Object.Instantiate(original, position, rotation, parent);
            InjectComponents(instance);

            return instance;
        }

        public void InjectComponents(GameObject instance)
        {
            // Inject components...
            foreach(var component in instance.GetComponents<Component>())
            {
                if (component != null)
                {
                    _componentService.Inject(component);
                }
            }

            // Recurse children...
            foreach(Transform child in instance.transform)
            {
                InjectComponents(child.gameObject);
            }
        }
    }
}
