using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    None,
    MainMenu,
    Gameplay,
    Victory,
    Defeat
}

public enum SFXType
{
    ButtonHover,
    ButtonClick,
    Hit,
    Death,
    Pickup,
    Explosion
}

public class AudioManager : MonoBehaviour
{
    public static class AudioPrefs
    {
        public const string MUSIC_VOLUME = "MusicVolume";
        public const string SFX_VOLUME = "SFXVolume";
        public const string MUSIC_MUTED = "MusicMuted";
        public const string SFX_MUTED = "SFXMuted";
    }

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

    public float MusicVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;

    public bool MusicMuted { get; private set; }
    public bool SFXMuted { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
        LoadAudioSetting();
    }
    private void LoadAudioSetting()
    {
        MusicVolume = PlayerPrefs.GetFloat(AudioPrefs.MUSIC_VOLUME, 1f);
        SFXVolume = PlayerPrefs.GetFloat(AudioPrefs.SFX_VOLUME, 1f);

        MusicMuted = PlayerPrefs.GetInt(AudioPrefs.MUSIC_MUTED, 0) == 1;
        SFXMuted = PlayerPrefs.GetInt(AudioPrefs.SFX_MUTED, 0) == 1;

        _musicSource.volume = MusicMuted ? 0 : MusicVolume;
        _sfxSource.volume = SFXMuted ? 0 : SFXVolume;
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
        MusicVolume = value;
        _musicSource.volume = MusicMuted ? 0 : value;

        PlayerPrefs.SetFloat(AudioPrefs.MUSIC_VOLUME, value);
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = value;
        _sfxSource.volume = SFXMuted ? 0 : value;

        PlayerPrefs.SetFloat(AudioPrefs.SFX_VOLUME, value);
    }
    public void ToggleMusic()
    {
        MusicMuted = !MusicMuted;
        _musicSource.volume = MusicMuted ? 0 : MusicVolume;

        PlayerPrefs.SetInt(AudioPrefs.MUSIC_MUTED, MusicMuted ? 1 : 0);
    }
    public void ToggleSFX()
    {
        SFXMuted = !SFXMuted;
        _sfxSource.volume = SFXMuted ? 0 : SFXVolume;

        PlayerPrefs.SetInt(AudioPrefs.SFX_MUTED, SFXMuted ? 1 : 0);
    }
}

