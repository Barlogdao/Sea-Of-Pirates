using UnityEngine;

namespace Project.Systems.Audio
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        private void Awake()
        {
            _musicSource.loop = true;
        }

        public void PlaySound(AudioClip clip)
        {
            _soundSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void MuteAudio()
        {
            _soundSource.mute = true;
            _musicSource.mute = true;
        }

        public void UnmuteAudio()
        {
            _soundSource.mute = false;
            _musicSource.mute = false;
        }

        public void PauseAudio()
        {
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }

        public void UnpauseAudio()
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }
    }
}