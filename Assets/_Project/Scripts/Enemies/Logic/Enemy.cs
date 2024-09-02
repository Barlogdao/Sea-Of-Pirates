using System;
using Cysharp.Threading.Tasks;
using Project.Configs.Enemies;
using Project.Enemies.View;
using Project.Interfaces.Audio;
using Project.Interfaces.Enemies;
using Project.Players.Logic;
using Project.Spawner;
using Project.Systems.Data;
using UnityEngine;

namespace Project.Enemies.Logic
{
    [RequireComponent(typeof(EnemyStateMachine), typeof(BoxCollider))]
    public class Enemy : MonoBehaviour, IPoolableEnemy
    {
        private const int MinimumHealth = 0;
        private const float ShootDelay = 0.1f;

        [SerializeField] private PlayerDetector _playerDetector;

        private EnemyConfig _config;
        private BoxCollider _shipCollider;
        private EnemyView _view;
        private VfxSpawner _vfxSpawner;
        private EnemyMover _mover;
        private EnemyStateMachine _stateMachine;
        private int _currentHealth;

        public event Action<IEnemy> Died;
        public event Action Respawned;
        public event Action Damaged;

        public bool IsAlive => _currentHealth > MinimumHealth;
        public int Damage => _config.Damage;
        public float AttackInterval => _config.AttackCooldown;

        public EnemyConfig Config => _config;
        public EnemyMover Mover => _mover;
        public Collider ShipCollider => _shipCollider;
        public PlayerDetector Detector => _playerDetector;
        public Transform Transform => transform;
        public Vector3 Position => transform.position;
        public GameResourceAmount Loot => _config.Loot;
        public Vector3 SpawnPosition { get; private set; }

        public void Initialize(
            EnemyConfig config,
            VfxSpawner vfxSpawner,
            Player player,
            IAudioService audioService)
        {
            SetSpawnPosition();
            _shipCollider = GetComponent<BoxCollider>();
            _stateMachine = GetComponent<EnemyStateMachine>();

            _config = config;
            _vfxSpawner = vfxSpawner;
            _mover = new EnemyMover(_config, transform);
            _view = Instantiate(_config.View, transform);
            _currentHealth = config.MaxHealth;

            SetShipColliderSize();

            _view.Initialize(_vfxSpawner, audioService);
            _playerDetector.Initialize(_config.DetectRange);
            _stateMachine.Initialize(player);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _view.TakeDamage();

            Damaged?.Invoke();

            if (!IsAlive)
                Die();
        }

        public async UniTaskVoid DealDamageAsync (Player player)
        {
            _view.Shoot(player.transform.position);
            await UniTask.Delay(TimeSpan.FromSeconds(ShootDelay),cancellationToken: destroyCancellationToken);
            player.TakeDamage(Damage);
        }

        public void Respawn(Vector3 atPosition)
        {
            gameObject.SetActive(true);
            transform.position = atPosition;
            SetSpawnPosition();
            RestoreHealth();
            _view.Ressurect();
            Respawned?.Invoke();
        }

        public void SetShipColliderSize()
        {
            _shipCollider.size = _view.ShipBounds.size;
            _shipCollider.center = _view.ShipBounds.center;
        }

        private void RestoreHealth()
        {
            _currentHealth = _config.MaxHealth;
        }

        private void SetSpawnPosition()
        {
            SpawnPosition = transform.position;
        }

        private void Die()
        {
            Died?.Invoke(this);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public async UniTask SinkAsync()
        {
            await _view.DieAsync();
            Deactivate();
        }
    }
}