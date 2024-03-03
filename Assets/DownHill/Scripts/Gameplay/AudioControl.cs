using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType { 
    music
    , sound
}

[RequireComponent(typeof(AudioSource))]
public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioType type;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        switch (type) { 
            case AudioType.music:
                GameManager.Instance.OnMusicVolumeChanged += SetVolume;
                SetVolume(GameManager.Instance.musicVolume);
            break;
            case AudioType.sound:
                GameManager.Instance.OnSoundVolumeChanged += SetVolume;
                SetVolume(GameManager.Instance.soundVolume);
                break;
        }
        
    }

    private void SetVolume(float volume) {
        audioSource.volume = volume;
    }

    private void OnDestroy()
    {
        switch (type)
        {
            case AudioType.music:
                GameManager.Instance.OnMusicVolumeChanged -= SetVolume;
                break;
            case AudioType.sound:
                GameManager.Instance.OnSoundVolumeChanged -= SetVolume;
                break;
        }
    }
}
