using Ami.BroAudio;
using UnityEngine;

namespace Project.Interfaces.Audio
{
    public interface IAudioService
    {
        void PlaySound(SoundID id);
        void PlayMusic(SoundID id);
        void PlayAmbience(SoundID id);
        void MuteAudio();
        void UnmuteAudio();
        void PauseAudio();
        void UnpauseAudio();
        void StopAudio();
    }
}
