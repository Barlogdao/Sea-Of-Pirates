using Ami.BroAudio;
using Project.Interfaces.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroAudioService : IAudioService
{
    public void PlayMusic(AudioClip clip)
    {
        throw new System.NotImplementedException();
    }

    public void PlaySound(AudioClip clip)
    {
        throw new System.NotImplementedException();
    }

    public void PlayMusic(SoundID id)
    {
        BroAudio.Play(id).AsBGM();
    }

    public void PlaySound(SoundID id)
    {
        BroAudio.Play(id);
    }

    public void MuteAudio()
    {
        BroAudio.SetVolume(0f);
    }

    public void UnmuteAudio()
    {
        BroAudio.SetVolume(1f);
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
