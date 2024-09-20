using System;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using Project.Configs.Enemies;
using Project.Configs.Level;
using Project.Enemies.View;
using Project.Interfaces.Audio;
using Project.Interfaces.Enemies;
using Project.Players.Logic;
using Project.Spawner;
using Project.Systems.Audio;
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
        [SerializeField] private AttackRangeView _attackRangeView;
        [SerializeField] private SoundID _deathSound;

        private EnemyConfig _config;
        private BoxCollider _shipCollider;
        private EnemyView _view;
        private VfxSpawner _vfxSpawner;
        private EnemyMover _mover;
        private EnemyStateMachine _stateMachine;
        private IAudioService _audioService;
        private int _currentHealth;

        public event Action<IEnemy> Died;
        public event Action Respawned;
        public event Action Damaged;
        public event Action<int, int> HealthChanged;

        public bool IsAlive => _currentHealth > MinimumHealth;
        public int Damage => _config.Damage;
        public float AttackCooldown => _config.AttackCooldown;

        public EnemyConfig Config => _config;
        public EnemyMover Mover => _mover;
        public EnemyView View => _view;
        public AttackRangeView AttackRangeView => _attackRangeView;
        public Collider ShipCollider => _shipCollider;
        public PlayerDetector Detector => _playerDetector;
        public Transform Transform => transform;
        public VfxSpawner VfxSpawner => _vfxSpawner;
        public Vector3 Position => transform.position;
        public GameResourceAmount Loot => _config.Loot;
        public Vector3 SpawnPosition { get; private set; }


        private void Start()
        {
            HealthChanged?.Invoke(_currentHealth, _config.MaxHealth);
        }

        public void Initialize(
            EnemyConfig config,
            VfxSpawner vfxSpawner,
            Player player,
            IAudioService audioService,
            LevelConfig levelConfig)
        {
            _config = config;
            _vfxSpawner = vfxSpawner;
            _audioService = audioService;
            _currentHealth = _config.MaxHealth;

            _shipCollider = GetComponent<BoxCollider>();
            _stateMachine = GetComponent<EnemyStateMachine>();
            _mover = new EnemyMover(_config, transform);
            _view = Instantiate(_config.View, transform);

            SetShipCollider(player);
            SetSpawnPosition();

            _view.Initialize(this, _vfxSpawner, _audioService, levelConfig);
            _attackRangeView.Initialize(_config.AttackRange);
            _playerDetector.Initialize(_config.DetectRange);
            _stateMachine.Initialize(player, _audioService);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            _view.TakeDamage(damage);

            Damaged?.Invoke();
            HealthChanged?.Invoke(_currentHealth, _config.MaxHealth);

            if (!IsAlive)
                Die();
        }

        public async UniTaskVoid DealDamageAsync(Player player)
        {
            _view.Shoot(player.transform.position);
            await UniTask.Delay(TimeSpan.FromSeconds(ShootDelay), cancellationToken: destroyCancellationToken);
            player.TakeDamage(Damage);
        }

        public void Respawn(Vector3 atPosition)
        {
            gameObject.SetActive(true);
            _shipCollider.enabled = true;

            transform.position = atPosition;
            SetSpawnPosition();
            RestoreHealth();
            _view.Ressurect();
            Respawned?.Invoke();
        }

        public void SetShipCollider(Player player)
        {
            if (_config.IsSolidForPlayer)
            {
                _shipCollider.includeLayers = 1 << player.PhysicsLayer;
            }

            _shipCollider.size = _view.ShipBounds.size;
            _shipCollider.center = _view.ShipBounds.center;
        }

        public void RestoreHealth()
        {
            _currentHealth = _config.MaxHealth;
            HealthChanged?.Invoke(_currentHealth, _config.MaxHealth);
        }

        private void SetSpawnPosition()
        {
            SpawnPosition = transform.position;
        }

        private void Die()
        {
            Died?.Invoke(this);
            _audioService.PlaySound(_deathSound);
            _shipCollider.enabled = false;
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