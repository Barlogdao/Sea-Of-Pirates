using UnityEngine;
using UnityEngine.UI;

namespace Project.Systems.Audio
{
    public class MuteController : MonoBehaviour
    {
        [SerializeField] private AudioService _audioService;
        [SerializeField] private Button _muteButton;
        [SerializeField] private Image _muteButtonImage;
        [SerializeField] private Sprite _muteIcon;
        [SerializeField] private Sprite _unmuteIcon;

        private bool _isMuted;

        private void Start()
        {
            _isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
            UpdateAudioState();
            UpdateButtonImage();

            _muteButton.onClick.AddListener(ToggleMute);
        }

        private void ToggleMute()
        {
            _isMuted = !_isMuted;
            UpdateAudioState();

            PlayerPrefs.SetInt("isMuted", _isMuted ? 1 : 0);
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
