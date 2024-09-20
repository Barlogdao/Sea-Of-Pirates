using System;
using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Interfaces.Audio;
using UnityEngine;


namespace Project.Enemies.View
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _splashParticles;
        [SerializeField] private MeshRenderer _projectileRenderer;
        [SerializeField] private SoundID _splashSound;

        private const float ScaleMiltiplier = 2f;
        private const float VisualRadiusOffset = 0.2f;

        private IAudioService _audioService;

        public void Initialize(IAudioService audioService)
        {
            _audioService ??= audioService;
        }

        public async UniTaskVoid ShootAsync(float radius, float explodeDelay, LayerMask targetMask, Action onReachCallback)
        {
            Vector3 scale = ScaleMiltiplier * (radius + VisualRadiusOffset) * Vector3.one;
            transform.localScale = Vector3.one;
            _projectileRenderer.enabled = true;


            await transform.DOScale(scale, explodeDelay);

            if (Physics.CheckSphere(transform.position, radius, targetMask))
            {
                onReachCallback();
            }
            else
            {
                _projectileRenderer.enabled = false;
                
                _splashParticles.Play();
                _audioService.PlaySound(_splashSound);
                await UniTask.WaitUntil (() => _splashParticles.isPlaying == false);
            }

            gameObject.SetActive(false);
        }
    }
}