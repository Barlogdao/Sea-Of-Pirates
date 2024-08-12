using System;
using Project.Configs.GameResources;
using Project.Interfaces.Enemies;
using UnityEngine;

namespace Project.Enemies
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private EnemyWandering _enemyWandering;
        
        private const int MinimumHealth = 0;

        private EnemyConfig _enemyConfig;

        private int _health;
        private int _damage;
        private GameResource _gameResource;
        private int _resourceAmount;

        public event Action<Enemy> Died;

        public void Initialize(EnemyConfig enemyConfig, Vector3 center)
        {
            _health = enemyConfig.Health;
            _damage = enemyConfig.Damage;
            _gameResource = enemyConfig.GameResource;
            _resourceAmount = enemyConfig.ResourceAmount;
            _enemyWandering.StartMoving(center);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;

            if (_health <= MinimumHealth)
            {
                Died?.Invoke(this);
            }
        }
    }
}