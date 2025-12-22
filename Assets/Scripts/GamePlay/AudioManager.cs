using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMusicVolume(float value)
    {
        _musicSource.volume = value;
        _musicSource.mute = value <= 0;
    }

    public void SetSFXVolume(float value)
    {
        _sfxSource.volume = value;
        _sfxSource.mute = value <= 0;
    }
}
