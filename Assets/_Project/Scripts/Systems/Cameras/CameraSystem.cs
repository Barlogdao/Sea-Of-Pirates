using System;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Project.Configs.Game;
using Project.Players.Logic;
using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Systems.Cameras
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private bool _isOpeningShows = true;
        [SerializeField] private float _openingViewDuration = 2f;
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _targetCamera;
        [SerializeField] private CinemachineVirtualCamera _openingCamera;

        private List<CinemachineVirtualCamera> _cameras;
        private CinemachineBrain _brain;
        private Player _player;
        private CinemachineTransposer _targetCameraTransposer;
        private UiCanvas _uiCanvas;
        private Vector3 _followOffset;

        [Inject]
        public void Construct(
            Player player, 
            CinemachineBrain brain,
            UiCanvas uiCanvas)
        {
            _player = player;
            _brain = brain;
            _uiCanvas = uiCanvas;

            _cameras = new List<CinemachineVirtualCamera>()
            {
                _playerCamera,
                _targetCamera,
                _openingCamera,
            };

            _targetCameraTransposer = _targetCamera.GetCinemachineComponent<CinemachineTransposer>();
            _followOffset = _targetCameraTransposer.m_FollowOffset;

            SetPlayerCamera();

            if (_isOpeningShows)
                EnableCamera(_openingCamera);
            else
                GoToPlayer();
        }

        public async UniTask ShowOpeningAsync(CancellationToken cts)
        {
            if (_isOpeningShows == false)
            {
                await UniTask.NextFrame(cancellationToken: cts);
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_openingViewDuration), cancellationToken: cts);
            GoToPlayer();
            await UniTask.WaitUntil(() => _brain.IsBlending == false, cancellationToken: cts);
        }

        public void GoToTarget(Transform target, CameraFollowOffset cameraFollowOffset = null)
        {
            _targetCameraTransposer.m_FollowOffset = cameraFollowOffset == null ? _followOffset : cameraFollowOffset.Value;

            SetTargetCamera(target);
            EnableCamera(_targetCamera);
        }

        public void GoToPlayer()
        {
            EnableCamera(_playerCamera);
        }

        public async UniTaskVoid ShowTarget(Transform target, float duration)
        {
            _uiCanvas.Disable();
            _player.DisableMove();
            GoToTarget(target);
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: destroyCancellationToken);
            GoToPlayer();
            await UniTask.WaitUntil(() => _brain.IsBlending == false, cancellationToken: destroyCancellationToken);
            _player.EnableMove();
            _uiCanvas.Enable();
        }

        private void SetPlayerCamera()
        {
            _playerCamera.Follow = _player.transform;
            _playerCamera.LookAt = _player.transform;
        }

        private void SetTargetCamera(Transform target)
        {
            _targetCamera.Follow = target;
            _targetCamera.LookAt = target;
        }

        private void EnableCamera(CinemachineVirtualCamera camera)
        {
            foreach (var virtualCamera in _cameras)
            {
                if (virtualCamera == camera)
                    virtualCamera.gameObject.SetActive(true);
                else
                    virtualCamera.gameObject.SetActive(false);
            }
        }
    }
}