using System.Collections;
using Assets.Scripts.MonoBehaviours.Player;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviours.ChargeableItems
{
    public class ChargeableItemAttackComponent : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _attackDelay = 3;
        [SerializeField] private float _attackRange = 3;
        [SerializeField] private string[] _attackedLayers;

        [SerializeField] private Transform _trackingComponent;

        public void StartAttack()
        {
            _hittedCount = GetEnemiesOnAttackRange();
            _isAttacking = true;

            StartCoroutine(nameof(StartAttackEnemy));
        }

        public void StopAttack()
        {
            _hittedCount = 0;
            _isAttacking = false;

            StopCoroutine(nameof(StartAttackEnemy));
        }

        private void FixedUpdate()
        {
            if (!_isAttacking) return;

            _hittedCount = GetEnemiesOnAttackRange();

            if (_hittedCount > 0)
            {
                _trackingComponent?.LookAt(_hitted[0].transform);
            }
        }

        private IEnumerator StartAttackEnemy()
        {
            while (true)
            {
                yield return new WaitForSeconds(_attackDelay);

                Attack();
            }
        }

        private void Attack()
        {
            if (_hittedCount <= 0) return;

            try
            {
                MakeDamage(_hitted[0]);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log($"Отслеживаемый объект уничтожен: + {e.Message}");
            }
        }

        private int GetEnemiesOnAttackRange()
        {
            return Physics.OverlapSphereNonAlloc(
                transform.position,
                _attackRange,
                _hitted,
                LayerMask.GetMask(_attackedLayers));
        }

        private void MakeDamage(Collider collider)
        {
            if (collider.TryGetComponent<HealthComponent>(out var health))
            {
                health.ChangeHealthCount(-_damage);
            }
        }

        private Collider[] _hitted = new Collider[50];
        private int _hittedCount = 0;

        private bool _isAttacking;
    }
}