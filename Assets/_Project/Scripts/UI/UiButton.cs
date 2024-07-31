using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class UiButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            Hide();
        }
        public void Show(Action onClickCallback)
        {
            gameObject.SetActive(true);
            _button.onClick.AddListener(onClickCallback.Invoke);
        }

        public void Hide()
        {
            _button.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}