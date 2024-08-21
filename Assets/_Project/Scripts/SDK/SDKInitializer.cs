using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.SDK
{
    public class SDKInitializer : MonoBehaviour
    {
        [SerializeField, Scene] private string _loadingScene;

        private IEnumerator Start()
        {
            yield return null;
#if !UNITY_EDITOR
        Agava.YandexGames.YandexGamesSdk.Initialize(LoadScene);
#else
            LoadScene();
#endif
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_loadingScene);
        }
    }
}