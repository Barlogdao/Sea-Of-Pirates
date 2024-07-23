using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Project.Enemies;
using Project.Interfaces.Stats;
using Zenject;

namespace Scripts.Players.PlayerLogic
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float _�ttackCoolDown = 3f;
        [SerializeField] private List<Gun> _gunList;
        [SerializeField] private GameObject AttackEffect;

        private List<Enemy> _attackList = new List<Enemy>();
        private Enemy _targetEnemy;
        private bool _isAttacking;
        private IPlayerStats _playerStats;

        private int Damage => _playerStats.Damage;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                _attackList.Add(enemy);
                if (!_isAttacking)
                {
                    StartCoroutine(StartAttack());
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                _attackList.Remove(enemy);
                if( _attackList.Count == 0)
                {
                    _isAttacking = false;
                }
            }
        }

        [Inject]
        public void Construct(IPlayerStats playerStats)
        {
            _playerStats = playerStats;
        }

        private bool CanAttack()
        {
            return !_isAttacking && CoolDownIsUp();
        }

        private bool CoolDownIsUp()
        {
            return _�ttackCoolDown <= 0;
        }

        private void Attack(Enemy enemy)
        {
            _isAttacking = true;
            for (int i = 0; i < _gunList.Count; i++)
            {
                _gunList[i].Attack();
            }
            enemy.TakeDamage(Damage);
        }

        private IEnumerator StartAttack()
        {
            _isAttacking = true;
            while (_isAttacking)
            {
                DetectingNearestUnit();
                if (CanAttack())
                {
                    Attack(_targetEnemy);
                    yield return new WaitForSeconds(_�ttackCoolDown);
                }
            }
        }

        private Enemy DetectingNearestUnit()
        {
            float minDistance = Mathf.Infinity;

            Enemy �losestEnemy = null;

            foreach (Enemy enemy in _attackList)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        �losestEnemy = enemy;
                    }
                }
            }

            if (�losestEnemy != null)
            {
                _targetEnemy = �losestEnemy;
            }
            return _targetEnemy;
        }
    }
}
