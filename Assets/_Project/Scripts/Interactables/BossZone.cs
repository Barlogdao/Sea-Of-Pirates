using Ami.BroAudio;
using Project.Interfaces.Audio;
using Project.Players.Logic;
using Project.Spawner;
using UnityEngine;
using Zenject;

namespace Project.Interactables
{
    public class BossZone : CameraViewZone
    {
        [SerializeField] private BossSpawner _bossSpawner;
        [SerializeField] private SoundID _enterZoneSound;

        private IAudioService _audioService;

        protected override void OnPlayerEntered(Player player)
        {
            base.OnPlayerEntered(player);

            if (_bossSpawner.IsBossSpawned)
            {
                _audioService.PlaySound(_enterZoneSound);
            }
        }

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }
    }
}