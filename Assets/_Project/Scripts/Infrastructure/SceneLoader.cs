using System;
using Cysharp.Threading.Tasks;
using Scripts.Interfaces.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Infrastructure
{
    public class SceneLoader : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private Image _loadingBar;

        private ILevelSceneService _levelSceneService;

        private async UniTaskVoid Start()
        {
            _loadingBar.fillAmount = 0;

            await SceneManager
                .LoadSceneAsync(_levelSceneService.CurrentLevel)
                .ToUniTask(progress: this);
        }

        public void Report(float value)
        {
            _loadingBar.fillAmount = value;
        }

        [Inject]
        private void Construct(ILevelSceneService levelSceneService)
        {
            _levelSceneService = levelSceneService;
        }
    }
}