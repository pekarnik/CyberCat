using System;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectComponentsExtensions
    {
        public static void IfNullLogError<T>(this T component) where T : Component
        {
            if (component == null)
            {
                Debug.LogError($"Component {typeof(T)} is null");
            }
        }

        public static T TryGetComponentInChildren<T>(this Transform transform) where T : Component
        {
            return transform.GetComponentInChildren<T>() ?? throw new NullReferenceException($"Game object {transform.name} does not have component {typeof(T)}");
        }
    }
}