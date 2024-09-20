using Project.Interfaces.Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Systems.Audio
{
    public class MuteController : MonoBehaviour
    {
        private const string MuteKey = nameof(MuteKey);

        [SerializeField] private Button _muteButton;
        [SerializeField] private Image _muteButtonImage;
        [SerializeField] private Sprite _muteIcon;
        [SerializeField] private Sprite _unmuteIcon;

        private IAudioService _audioService;
        private bool _isMuted;

        private void Start()
        {
            _isMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
            UpdateAudioState();
            UpdateButtonImage();

            _muteButton.onClick.AddListener(ToggleMute);
        }

        [Inject]
        public void Construct(IAudioService audioService)
        {
            _audioService = audioService;
        }

        private void ToggleMute()
        {
            _isMuted = !_isMuted;
            UpdateAudioState();

            PlayerPrefs.SetInt(MuteKey, _isMuted ? 1 : 0);
            PlayerPrefs.Save();
            UpdateButtonImage();
        }

        private void UpdateAudioState()
        {
            if (_isMuted)
            {
                _audioService.MuteAudio();
            }
            else
            {
                _audioService.UnmuteAudio();
            }
        }

        private void UpdateButtonImage()
        {
            _muteButtonImage.sprite = _isMuted ?  _muteIcon : _unmuteIcon;
        }
    }
}
