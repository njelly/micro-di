using System;
using UnityEngine;

namespace MicroDI.Helpers
{
    /// <summary>
    /// Provides registerable callbacks into
    /// Unity update methods for non-monobehaviour usage.
    /// </summary>
    public class UpdateService : MonoBehaviour
    {
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}
