using MicroDI.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MicroDI.Helpers
{
    public class ComponentService
    {
        private static Dictionary<Type, IEnumerable<MemberInfo>> _cachedPathInjectMembers = new Dictionary<Type, IEnumerable<MemberInfo>>();

        private readonly IContainer _container;

        public ComponentService(IContainer container)
        {
            _container = container;
        }

        public void Inject(Component component)
        {
            if (component == null)
                return;

            // Injects [Inject]...
            _container.Injector.InjectMembers(component);
        }
    }
}
