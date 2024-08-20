using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class NextLevelWindow : UiWindow
    {
        [SerializeField] private TMP_Text _windowText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private RectTransform _endGamePanel;

        [SerializeField] private Image _mapImage;
        [SerializeField] private Color _hasMapColor = Color.white;
        [SerializeField] private Color _noMapColor = Color.black;

        protected override void Awake()
        {
            base.Awake();
            _endGamePanel.gameObject.SetActive(false);
        }

        public void Open(string windowText, bool hasMap, Action onConfirmCallback)
        {
            base.Show();

            _windowText.text = windowText;
            _mapImage.color = hasMap ? _hasMapColor : _noMapColor;

            _confirmButton.onClick.AddListener(onConfirmCallback.Invoke);
        }

        public void ShowEndGamePanel()
        {
            _endGamePanel.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            _confirmButton.onClick.RemoveAllListeners();
            base.Hide();
        }

        protected override void OnDestroy()
        {
            _confirmButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }
    }
}