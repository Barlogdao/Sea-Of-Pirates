using System.Collections;
using Ami.BroAudio;
using DG.Tweening;
using Project.Interfaces.Audio;
using Project.Spawner;
using Project.Utils.Tweens;
using UnityEngine;
using Zenject;

namespace Project.Players.View
{
    public class PlayerAttackView : MonoBehaviour
    {
        private const float MinProgress = 0f;
        private const float MaxProgress = 1f;

        [SerializeField] private SoundID _shootSound;
        [SerializeField] private ShipAttackCones _attackCones;
        [SerializeField] private AppearingTransformTween _appearingTween;
        [SerializeField] private Collider _shipCollider;
        [SerializeField] private Ease _reloadEase = Ease.InOutSine;
        [SerializeField] private float _unloadDuration = 0.15f;

        private VfxSpawner _vfxSpawner;
        private IAudioService _audioService;

        [Inject]
        public void Construct(VfxSpawner vfxSpawner, IAudioService audioService)
        {
            _vfxSpawner = vfxSpawner;
            _audioService = audioService;
            _appearingTween.Initialize(_attackCones.transform);
            _attackCones.gameObject.SetActive(false);
        }

        public void Show()
        {
            _attackCones.gameObject.SetActive(true);
            _appearingTween.Appear();
        }

        public void Hide()
        {
            _appearingTween.Disappear(() => _attackCones.gameObject.SetActive(false));
        }

        public void SetRange(float attackRange)
        {
            _attackCones.SetRadius(attackRange);
        }

        public void SetAngle(float angle)
        {
            _attackCones.SetAngle(angle);
        }

        public void Shoot(Vector3 targetPosition)
        {
            _vfxSpawner.SpawnCannonSmoke(_shipCollider, targetPosition);
            _audioService.PlaySound(_shootSound);

        }

        public IEnumerator CannonsLoading(float cooldown)
        {
            yield return DOTween.To(SetProgress, MinProgress, MaxProgress, cooldown)
                .SetEase(_reloadEase)
                .WaitForKill();
        }

        public IEnumerator CannonsUnloading()
        {
            yield return DOTween.To(SetProgress, MaxProgress, MinProgress, _unloadDuration)
                .SetEase(Ease.Linear)
                .WaitForKill();
        }

        private void SetProgress(float progress)
        {
            _attackCones.SetProgress(progress);
        }
    }
}