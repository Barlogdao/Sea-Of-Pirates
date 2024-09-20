using Project.Interfaces.Audio;
using Project.Systems.Audio;
using UnityEngine;

namespace Project.Systems.Pause
{
    public class PauseService
    {
        private readonly IAudioService _audioService;

        public PauseService(IAudioService audioService)
        {
            _audioService = audioService;
        }

        public void Pause()
        {
            _audioService.PauseAudio();
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            _audioService.UnpauseAudio();
            Time.timeScale = 1f;
        }
    }
}