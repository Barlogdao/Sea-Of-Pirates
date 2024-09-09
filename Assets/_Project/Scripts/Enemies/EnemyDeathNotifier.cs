using System;
using System.Collections.Generic;
using Project.Configs.Enemies;
using Project.Interfaces.Enemies;
using Project.Spawner;

namespace Project.Enemies
{
    public class EnemyDeathNotifier: IEnemyDeathNotifier, IDisposable
    {
        private readonly List<BaseEnemySpawner> _enemySpawners;

        public event Action<EnemyConfig> EnemyDied;

        public EnemyDeathNotifier(List<BaseEnemySpawner> enemySpawners)
        {
            _enemySpawners = enemySpawners;

            foreach (var enemySpawner in _enemySpawners)
            {
                enemySpawner.EnemyDied += OnEnemyDied;
            }
        }

        public void Dispose()
        {
            foreach (var enemySpawner in _enemySpawners)
            {
                enemySpawner.EnemyDied -= OnEnemyDied;
            }
        }

        private void OnEnemyDied(EnemyConfig enemyConfig)
        {
            EnemyDied?.Invoke(enemyConfig);
        }
    }
}