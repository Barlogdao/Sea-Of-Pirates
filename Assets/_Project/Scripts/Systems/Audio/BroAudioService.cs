using Ami.BroAudio;
using Project.Interfaces.Audio;
using UnityEngine;

public class BroAudioService : IAudioService
{
    private const float FadeTime = 0.1f;
    public void PlayMusic(SoundID id)
    {
        BroAudio.Play(id).AsBGM();
    }

    public void PlayAmbience(SoundID id)
    {
        BroAudio.Stop(BroAudioType.Ambience);
        BroAudio.Play(id);
    }

    public void PlaySound(SoundID id)
    {
        BroAudio.Play(id);
    }

    public void MuteAudio()
    {
        BroAudio.SetVolume(BroAudioType.All,0f, FadeTime);
    }

    public void UnmuteAudio()
    {
        BroAudio.SetVolume(BroAudioType.All, 1f, FadeTime);
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

    public void StopAudio()
    {
        BroAudio.Stop(BroAudioType.All);
    }
}