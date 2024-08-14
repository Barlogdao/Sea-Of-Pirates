using Project.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UiWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ScaleTween _scaleTween;

        private Canvas _windowCanvas;

        protected virtual void Awake()
        {
            _windowCanvas = GetComponent<Canvas>();

            _scaleTween.Initialize(transform);
            _closeButton.onClick.AddListener(Hide);
            Hide();
        }

        protected virtual void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Hide);
        }

        public virtual void Hide()
        {
            _windowCanvas.enabled = false;
        }
        protected void Show()
        {
            _windowCanvas.enabled = true;
            _scaleTween.Run();
        }
    }
}