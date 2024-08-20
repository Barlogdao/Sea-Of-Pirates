using Project.Configs.UI;
using Project.Interfaces.Audio;
using Project.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UiWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ScaleTween _scaleTween;

        private IAudioService _audioService;
        private AudioClip _openWindowSound;
        private AudioClip _closeWindowSound;

        private Canvas _windowCanvas;

        private bool IsClosed => _windowCanvas.enabled == false;

        protected virtual void Awake()
        {
            _windowCanvas = GetComponent<Canvas>();

            _scaleTween.Initialize(transform);
            _closeButton.onClick.AddListener(Hide);
            Close();
        }

        protected virtual void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Hide);
        }

        public virtual void Hide()
        {
            if (IsClosed)
                return;

            _audioService.PlaySound(_closeWindowSound);
            Close();
        }

        private void Close()
        {
            _windowCanvas.enabled = false;
        }

        protected void Show()
        {
            _windowCanvas.enabled = true;
            _audioService.PlaySound(_openWindowSound);
            _scaleTween.Run();
        }

        [Inject]
        private void Construct(IAudioService audioService, UiConfigs uiConfigs)
        {
            _audioService = audioService;
            _openWindowSound = uiConfigs.OpenWindowSound;
            _closeWindowSound = uiConfigs.CloseWindowSound;
        }
    }
}