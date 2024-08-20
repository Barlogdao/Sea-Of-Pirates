using Lean.Localization;
using Project.Configs.Game;
using Project.Configs.GameResources;
using Project.Configs.Level;
using Project.Interfaces.Data;
using Project.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Systems.Interactables
{
    public class NextLevelZone : InteractableZone
    {
        private const int MapAmount = 1;

        [SerializeField] private GameResource _map;
        [SerializeField, LeanTranslationName] private string _avaliablePassToken;
        [SerializeField, LeanTranslationName] private string _unavaliablePassToken;

        private IPlayerStorage _storage;
        private ILevelSceneService _levelSceneService;
        private NextLevelWindow _window;
        private GameConfig _gameConfig;
        private LevelConfig _levelConfig;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player _))
            {
                bool hasMap = _storage.CanSpend(_map, MapAmount);

                string windowText = hasMap ?
                    GetTranslatedText(_avaliablePassToken) :
                    GetTranslatedText(_unavaliablePassToken);

                if (hasMap)
                    _window.Open(windowText, hasMap, SwitchLevel);
                else
                    _window.Open(windowText, hasMap, () => _window.Hide());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out Player _))
                _window.Hide();
        }

        [Inject]
        private void Construct(
            IPlayerStorage storage,
            ILevelSceneService levelSceneService,
            NextLevelWindow window,
            GameConfig gameConfig,
            LevelConfig levelConfig)
        {
            _storage = storage;
            _levelSceneService = levelSceneService;
            _window = window;
            _gameConfig = gameConfig;
            _levelConfig = levelConfig;
        }

        private void SwitchLevel()
        {
            if (_levelConfig.IsLastLevel)
            {
                _window.ShowEndGamePanel();
                return;
            }

            _storage.TrySpendResource(_map, MapAmount);
            _levelSceneService.UpdateCurrentLevel(_levelConfig.NextLevel);
            DG.Tweening.DOTween.KillAll();
            SceneManager.LoadScene(_gameConfig.LoadingScene);
        }

        private string GetTranslatedText(string token)
        {
            return LeanLocalization.GetTranslationText(token);
        }
    }
}