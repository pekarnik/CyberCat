using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.Player
{
    public class DamageAbility : MonoBehaviour
    {
        [SerializeField, Range(1,10)] private int _damage;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<HealthComponent>(out var health))
            {
                health.ChangeHealthCount(-_damage);
            }
        }
    }
}