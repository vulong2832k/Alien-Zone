using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    None,
    MainMenu,
    Gameplay,
    Boss,
    Victory,
    Defeat
}

public enum SFXType
{
    ButtonClick,
    Hit,
    Death,
    Pickup,
    Explosion
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class MusicEntry
    {
        public MusicType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SFXEntry
    {
        public SFXType type;
        public AudioClip clip;
    }

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private List<MusicEntry> _musicClips;
    [SerializeField] private List<SFXEntry> _sfxClips;

    private Dictionary<MusicType, AudioClip> _musicDict;
    private Dictionary<SFXType, AudioClip> _sfxDict;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    private void Init()
    {
        _musicDict = new();

        foreach (var music in _musicClips)
        {
            _musicDict[music.type] = music.clip;
        }

        _sfxDict = new();
        foreach (var sfx in _sfxClips)
        {
            _sfxDict[sfx.type] = sfx.clip;
        }
    }

    public void PlayMusic(MusicType type)
    {
        if (type == MusicType.None) return;
        if (!_musicDict.ContainsKey(type)) return;

        AudioClip clip = _musicDict[type];

        if (_musicSource.clip == clip) return;

        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySFX(SFXType type)
    {
        if (!_sfxDict.ContainsKey(type)) return;
        _sfxSource.PlayOneShot(_sfxDict[type]);
    }

    public void SetMusicVolume(float value)
    {
        _musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        _sfxSource.volume = value;
    }
}

