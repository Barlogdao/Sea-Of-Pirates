using Project.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class UiButton : MonoBehaviour
    {
        [SerializeField] private ScaleTween _scaleTween;

        private Button _button;

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void Show(Action onClickCallback)
        {
            gameObject.SetActive(true);
            _scaleTween.Run();
            _button.onClick.AddListener(onClickCallback.Invoke);
        }

        public void Hide()
        {
            _button.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

        [Inject]
        private void Construct()
        {
            _button = GetComponent<Button>();
            _scaleTween.Initialize(transform);
        }
    }
}