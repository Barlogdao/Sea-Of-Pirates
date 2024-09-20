using Ami.BroAudio;
using Project.Interfaces.Audio;
using Project.Interfaces.Hold;
using Project.Interfaces.Stats;
using Project.Players.View;
using System;
using UnityEngine;
using Zenject;

namespace Project.Players.Logic
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerView _view;
        [SerializeField] private SoundID _healSound;

        private IPlayerStats _playerStats;
        private IPlayerHold _playerHold;
        private IAudioService _audioService;
        private Rigidbody _rigidbody;

        private int _currentHealth;
        private bool _canMove = true;

        public event Action HealthChanged;
        public event Action Died;

        public Vector3 Position => transform.position;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _playerStats.MaxHealth;
        public int PhysicsLayer => gameObject.layer;
        public bool IsAlive => _currentHealth > 0;
        public bool CanMove => _canMove;

        private void Start()
        {
            HealthChanged?.Invoke();
        }

        private void OnDestroy()
        {
            _playerStats.StatsUpdated -= OnStatsUpdated;
        }

        [Inject]
        public void Construct(
            IPlayerStats playerStats,
            IPlayerHold playerHold,
            IAudioService audioService)
        {
            _playerStats = playerStats;
            _playerHold = playerHold;
            _audioService = audioService;
            _currentHealth = MaxHealth;
            _rigidbody = GetComponent<Rigidbody>();

            _playerStats.StatsUpdated += OnStatsUpdated;
        }

        public void TakeDamage(int damage)
        {
            if (IsAlive == false)
                return;

            _currentHealth = Math.Max(_currentHealth - damage, 0);

            HealthChanged?.Invoke();
            _view.TakeDamage(damage);

            if (IsAlive == false)
                Died?.Invoke();
        }

        public void Heal()
        {
            if (_currentHealth != MaxHealth)
                _audioService.PlaySound(_healSound);

            _currentHealth = MaxHealth;

            HealthChanged?.Invoke();
        }

        public void UnloadHold()
        {
            _playerHold.LoadToStorage();
        }

        public void EnableMove()
        {
            _canMove = true;
        }

        public void DisableMove()
        {
            _canMove = false;
        }

        public void SetPosition(Vector3 at)
        {
            _rigidbody.MovePosition(at);
        }

        private void OnStatsUpdated()
        {
            Heal();
        }
    }
}