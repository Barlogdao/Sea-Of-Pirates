using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Interfaces.Enemies;
using Project.Interfaces.Stats;
using Project.Players.View;
using UnityEngine;
using Zenject;

namespace Project.Players.Logic
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PlayerAttackView _attackView;
        [SerializeField] private float _attackAngle = 100;
        [SerializeField] private float _attackCooldown = 2f;

        private readonly List<IEnemy> _trackedEnemies = new();
        private readonly WaitForSeconds _shootDelay = new(0.08f);

        private SphereCollider _detectZone;
        private IPlayerStats _playerStats;
        private Coroutine _battleCoroutine;
        private WaitUntil _hasTargetAwaiter;
        public event Action<IEnemy> EnemyKilled;

        public int Damage => _playerStats.Damage;
        public int AttackRange => _playerStats.AttackRange;
        public int CannonsAmount => _playerStats.CannonsAmount;

        private bool HasTrackedEnemies => _trackedEnemies.Count > 0;
        private bool IsInBattle => _battleCoroutine != null;
        private float HalfAttackAngle => _attackAngle / 2f;

        private void Start()
        {
            SetAttackZone();
        }

        private void OnEnable()
        {
            _detectZone.enabled = true;
        }

        private void OnDisable()
        {
            _detectZone.enabled = false;

            ClearTrackedEnemies();
            ExitBattle();
        }

        private void OnDestroy()
        {
            _playerStats.StatsUpdated -= OnStatsUpdated;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEnemy enemy) && enemy.IsAlive)
            {
                TrackEnemy(enemy);
                TryEnterBattle();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IEnemy enemy))
            {
                UntrackEnemy(enemy);
                TryExitBattle();
            }
        }

        [Inject]
        private void Construct(IPlayerStats playerStats)
        {
            _detectZone = GetComponent<SphereCollider>();
            _playerStats = playerStats;
            _hasTargetAwaiter = new WaitUntil(HasTargetEnemies);

            _playerStats.StatsUpdated += OnStatsUpdated;
        }

        private void SetAttackZone()
        {
            _detectZone.radius = AttackRange;
            _attackView.SetRange(AttackRange);
            _attackView.SetAngle(_attackAngle);
        }

        private void TrackEnemy(IEnemy enemy)
        {
            enemy.Died += OnEnemyDied;
            _trackedEnemies.Add(enemy);
        }

        private void UntrackEnemy(IEnemy enemy)
        {
            _trackedEnemies.Remove(enemy);
            enemy.Died -= OnEnemyDied;
        }

        private bool TryEnterBattle()
        {
            if (IsInBattle)
                return false;

            EnterBattle();
            return true;
        }

        private void EnterBattle()
        {
            _battleCoroutine = StartCoroutine(Battle());
            _attackView.Show();
        }

        private bool TryExitBattle()
        {
            if (HasTrackedEnemies)
                return false;

            ExitBattle();
            return true;
        }

        private void ExitBattle()
        {
            ClearBattleCoroutine();
            _attackView.Hide();
        }

        private void ClearBattleCoroutine()
        {
            if (_battleCoroutine != null)
                StopCoroutine(_battleCoroutine);

            _battleCoroutine = null;
        }

        private IEnumerator Battle()
        {
            while (HasTrackedEnemies)
            {
                yield return _attackView.CannonsLoading(_attackCooldown);

                yield return _hasTargetAwaiter;

                foreach (IEnemy enemy in GetTargetEnemies())
                {
                    _attackView.Shoot(enemy.Position);
                    yield return _shootDelay;
                    enemy.TakeDamage(Damage);
                }

                yield return _attackView.CannonsUnloading();
            }

            ExitBattle();
        }

        private bool HasTargetEnemies()
        {
            return _trackedEnemies.Any(enemy => CanAttackEnemy(enemy));
        }

        private IEnumerable<IEnemy> GetTargetEnemies()
        {
            return _trackedEnemies
                .Where(enemy => CanAttackEnemy(enemy))
                .OrderBy(enemy => Vector3.SqrMagnitude(enemy.Position - transform.position))
                .Take(Mathf.Min(_trackedEnemies.Count(), CannonsAmount));
        }

        private bool CanAttackEnemy(IEnemy enemy)
        {
            if (enemy.IsAlive == false)
                return false;

            Vector3 direction = enemy.Position - transform.position;
            float rightAngle = Vector3.Angle(transform.right, direction);
            float leftAngle = Vector3.Angle(-transform.right, direction);

            return rightAngle <= HalfAttackAngle || leftAngle <= HalfAttackAngle;
        }

        private void ClearTrackedEnemies()
        {
            foreach (var enemy in _trackedEnemies)
                enemy.Died -= OnEnemyDied;

            _trackedEnemies.Clear();
        }

        private void OnEnemyDied(IEnemy enemy)
        {
            EnemyKilled?.Invoke(enemy);
            UntrackEnemy(enemy);
        }

        private void OnStatsUpdated()
        {
            SetAttackZone();
        }
    }
}