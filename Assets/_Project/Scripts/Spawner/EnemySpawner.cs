using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Configs.Enemies;
using Project.Interfaces.Enemies;
using Project.Systems.Quests;
using Project.Utils.Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private bool _isRespawnable = true;
        [SerializeField, Min(1)] private int _maxEnemies;
        [SerializeField] private float _spawnRadius;
        [SerializeField, Min(3f)] private float _respawnDelay;
        [SerializeField] private LayerMask _obstaclesMask;
        [SerializeField] private QuestEnemyMark _questMark;

        private readonly List<IPoolableEnemy> _enemies = new();

        private EnemyFactory _enemyFactory;
        private WaitForSeconds _respawnCooldown;
        private bool _isEnemiesMarked = false;

        public EnemyConfig EnemyType => _enemyConfig;

        public event Action<EnemyConfig> EnemyDied;

        private void OnDestroy()
        {
            foreach (IPoolableEnemy enemy in _enemies)
                enemy.Died -= OnEnemyDied;
        }

        public void MarkEnemies()
        {
            if (_isEnemiesMarked)
                return;

            foreach (IPoolableEnemy enemy in _enemies)
            {
                float shipHeight = _enemyConfig.View.ShipBounds.max.y;
                Vector3 markPosition = enemy.Position.AddY(shipHeight);
                Instantiate(_questMark, markPosition, Quaternion.identity, enemy.Transform);
            }

            _isEnemiesMarked = true;
        }

        public void UnmarkEnemies()
        {
            foreach (IPoolableEnemy enemy in _enemies)
            {
                QuestEnemyMark mark = enemy.Transform.GetComponentInChildren<QuestEnemyMark>();

                if (mark != null)
                    Destroy(mark.gameObject);
            }

            _isEnemiesMarked = false;
        }

        [Inject]
        private void Construct(EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _respawnCooldown = new WaitForSeconds(_respawnDelay);

            for (int i = 0; i < _maxEnemies; i++)
            {
                Spawn();
            }

        }

        private void Spawn()
        {
            Vector3 enemyPosition = GetSpawnPosition();
            IPoolableEnemy enemy = _enemyFactory.Create(_enemyConfig, enemyPosition, transform);

            _enemies.Add(enemy);
            enemy.Died += OnEnemyDied;
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 position = GetRandomSpawnPosition();
            Bounds shipBounds = _enemyConfig.View.ShipBounds;

            while (Physics.CheckBox(position, shipBounds.extents, Quaternion.identity, _obstaclesMask))
            {
                position = GetRandomSpawnPosition();
            }

            return position;
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return (transform.position + Random.insideUnitSphere * _spawnRadius).WithZeroY();
        }

        private void OnEnemyDied(IEnemy enemy)
        {
            EnemyDied?.Invoke(_enemyConfig);

            if (_isRespawnable)
                StartCoroutine(Respawning(enemy as IPoolableEnemy));
        }

        private IEnumerator Respawning(IPoolableEnemy enemy)
        {
            yield return enemy.SinkAsync().ToCoroutine();
            yield return _respawnCooldown;

            enemy.Respawn(GetSpawnPosition());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
        }
    }
}