using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler
{
    private const float OffVolume = -80f;
    private const float OnVolume = 0f;

    private const string MusicKey = "MusicVolume";
    private const string SoundFXKey = "SoundFXVolume";

    private const int OnVolumeSaveKey = 1;
    private const int OffVolumeSaveKey = -1;

    private AudioMixerGroup _masterGroup;

    public AudioHandler(AudioMixerGroup masterGroup)
    {
        _masterGroup = masterGroup;
    }

    public void Initialize()
    {
        int musicSaveKey = PlayerPrefs.GetInt(MusicKey);

        if (musicSaveKey == 0 || musicSaveKey == OnVolumeSaveKey)
            OnMusic();
        else
            OffMusic();

        int soundFXSaveKey = PlayerPrefs.GetInt(SoundFXKey);

        if(soundFXSaveKey == 0 || soundFXSaveKey == OnVolumeSaveKey )
            OnSoundFX();
        else 
            OffSoundFX();
    }

    public bool IsMusicOn() => PlayerPrefs.GetInt(MusicKey) == OnVolumeSaveKey;
    public bool IsSoundFXOn() => PlayerPrefs.GetInt(SoundFXKey) == OnVolumeSaveKey;

    public void OffMusic()
    {
        _masterGroup.audioMixer.SetFloat(MusicKey, OffVolume);
        PlayerPrefs.SetInt(MusicKey, OffVolumeSaveKey);
    }

    public void OnMusic()
    {
        _masterGroup.audioMixer.SetFloat(MusicKey, OnVolume);
        PlayerPrefs.SetInt(MusicKey, OnVolumeSaveKey);
    }

    public void OffSoundFX()
    {
        _masterGroup.audioMixer?.SetFloat(SoundFXKey, OffVolume);
        PlayerPrefs.SetInt(SoundFXKey, OffVolumeSaveKey);
    }

    public void OnSoundFX()
    {
        _masterGroup.audioMixer.SetFloat(SoundFXKey, OnVolume);
        PlayerPrefs.SetInt(SoundFXKey, OnVolumeSaveKey);
    }
}
