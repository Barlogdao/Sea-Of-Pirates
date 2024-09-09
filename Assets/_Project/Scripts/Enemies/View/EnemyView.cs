using Project.Configs.Level;
using Project.Enemies.Logic;
using Project.General.View;
using Project.Interfaces.Audio;
using Project.Spawner;
using Project.Utils.Tweens;
using UnityEngine;

namespace Project.Enemies.View
{

    public class EnemyView : ShipView
    {
        [SerializeField] private PunchShipTween _punchTween;
        [SerializeField] private MeshFilter _shipMesh;
        [SerializeField] private MeshRenderer _shipRenderer;
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _hitSound;
        [SerializeField] private EnemyHudView _hudView;


        private VfxSpawner _vfxSpawner;
        private IAudioService _audioService;

        public Bounds ShipBounds => _shipMesh.sharedMesh.bounds;

        public void Initialize(
            Enemy enemy,
            VfxSpawner vfxSpawner,
            IAudioService audioService,
            LevelConfig levelConfig)
        {
            _vfxSpawner = vfxSpawner;
            _audioService = audioService;
            _hudView.Initialize(enemy);
            _punchTween.Initialize(transform);

            SetRenderers(levelConfig);
        }

        public override void TakeDamage(int damage)
        {
            _vfxSpawner.SpawnExplosion(transform.position, transform);
            _audioService.PlaySound(_hitSound);
            _vfxSpawner.ShowDamage(transform.position, damage);
            _punchTween.Punch();
        }

        public void Shoot(Vector3 targetPosition)
        {
            _vfxSpawner.SpawnCannonSmoke(transform.position, targetPosition);
            _audioService.PlaySound(_shootSound);
        }

        public void ShowHud()
        {
            _hudView.Show();
        }

        public void HideHud()
        {
            _hudView.Hide();
        }

        private void SetRenderers(LevelConfig levelConfig)
        {
            Material levelMaterial = levelConfig.LevelMaterial;

            if (_shipRenderer.material == levelMaterial)
                return;

            _shipRenderer.material = levelMaterial;
        }
    }
}