using Cysharp.Threading.Tasks;
using Project.Interfaces.SDK;
using Project.Players.Logic;
using Project.Spawner;
using Project.Systems.Cameras;
using Project.Systems.Quests;
using Project.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Infrastructure
{
    public class StartLevelHandler : MonoBehaviour
    {
        [SerializeField] private float _unfadeCanvasDuration = 0.5f;
        [SerializeField] private GameObject _pointerCanvas;

        private List<BaseEnemySpawner> _enemySpawners;
        private QuestEnemyHandler _questEnemyHandler;
        private Player _player;
        private PlayerSpawner _playerSpawner;
        private CameraSystem _cameraSystem;
        private UiCanvas _uiCanvas;
        private IGameReadyService _gameReadyService;
        private JoystickCanvas _joystickCanvas;

        [Inject]
        public void Construct(
            List<BaseEnemySpawner> enemySpawners,
            QuestEnemyHandler questEnemyHandler,
            Player player,
            PlayerSpawner playerSpawner,
            CameraSystem cameraSystem,
            UiCanvas uiCanvas,
            IGameReadyService gameReadyService,
            JoystickCanvas joystickCanvas = null)
        {
            _enemySpawners = enemySpawners;
            _questEnemyHandler = questEnemyHandler;
            _player = player;
            _playerSpawner = playerSpawner;
            _cameraSystem = cameraSystem;
            _uiCanvas = uiCanvas;
            _gameReadyService = gameReadyService;
            _joystickCanvas = joystickCanvas;
        }

        private async UniTaskVoid Start()
        {
            _playerSpawner.Initialize();
            _player.DisableMove();
            DisableUi();

            await _cameraSystem.ShowOpeningAsync(destroyCancellationToken);

            _player.EnableMove();
            EnableUi();

            _gameReadyService.Call();

            await _uiCanvas.EnableAsync(_unfadeCanvasDuration, destroyCancellationToken);

            PrepareSpawners();
            InitializeQuestEnemies();
        }

        private void InitializeQuestEnemies()
        {
            _questEnemyHandler.Initialize();
        }

        private void PrepareSpawners()
        {
            foreach (var spawner in _enemySpawners)
            {
                spawner.Prepare();
            }
        }

        private void EnableUi()
        {
            if (_joystickCanvas != null)
                _joystickCanvas.Enable();

            if (_pointerCanvas != null)
                _pointerCanvas.SetActive(true);
        }

        private void DisableUi()
        {
            _uiCanvas.Disable();

            if (_joystickCanvas != null)
                _joystickCanvas.Disabe();

            if (_pointerCanvas != null)
                _pointerCanvas.SetActive(false);
        }
    }
}