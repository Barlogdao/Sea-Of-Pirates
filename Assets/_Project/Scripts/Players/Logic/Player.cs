using Project.Interfaces.Audio;
using Project.Interfaces.Hold;
using Project.Interfaces.Stats;
using Project.Spawner;
using System;
using UnityEngine;
using Zenject;

namespace Project.Players.Logic
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;

        private IPlayerStats _playerStats;
        private IAudioService _audioService;
        private IPlayerHold _playerHold;
        private VfxSpawner _vfxSpawner;

        private int _currentHealth;

        public event Action HealthChanged;
        public event Action Died;

        public Vector3 Position => transform.position;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _playerStats.MaxHealth;
        public bool IsAlive => _currentHealth > 0;

        private void Start()
        {
            HealthChanged?.Invoke();
        }

        [Inject]
        public void Construct(
            IPlayerStats playerStats,
            IAudioService audioService,
            IPlayerHold playerHold,
            VfxSpawner vfxSpawner)
        {
            _playerStats = playerStats;
            _audioService = audioService;
            _playerHold = playerHold;
            _vfxSpawner = vfxSpawner;
            _currentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (IsAlive == false)
                return;

            _currentHealth = Math.Max(_currentHealth - damage, 0);

            HealthChanged?.Invoke();
            ShowHitEffect();

            if (IsAlive == false)
                Died?.Invoke();
        }

        public void Heal()
        {
            _currentHealth = MaxHealth;
            HealthChanged?.Invoke();
        }

        public void UnloadHold()
        {
            _playerHold.LoadToStorage();
        }

        private void ShowHitEffect()
        {
            _audioService.PlaySound(_audioClip);
            _vfxSpawner.SpawnExplosion(transform.position, transform);
        }
    }
}