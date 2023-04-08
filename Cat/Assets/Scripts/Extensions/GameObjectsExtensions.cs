using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectsExtensions
    {
        public static T TryGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out var result))
            {
                Debug.Log($"Component {typeof(T)} not set at game object and added in progress");
                result = gameObject.AddComponent<T>();
            }

            return result;
        }
    }
}