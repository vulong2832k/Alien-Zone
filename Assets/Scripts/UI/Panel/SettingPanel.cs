using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _settingPanel;

    [Header("Music")]
    [SerializeField] private Image _musicOnImg;
    [SerializeField] private Image _musicOffImg;
    [SerializeField] private Slider _musicSlider;

    [Header("SFX")]
    [SerializeField] private Image _sfxOnImg;
    [SerializeField] private Image _sfxOffImg;
    [SerializeField] private Slider _sfxSlider;

    [Header("Button")]
    [SerializeField] private Button _exitBtn;
    private void OnEnable()
    {
        SyncUI();
    }
    private void Awake()
    {
        _exitBtn.onClick.AddListener(Hide);

        _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        _musicOnImg.GetComponent<Button>().onClick.AddListener(OnToggleMusic);
        _musicOffImg.GetComponent<Button>().onClick.AddListener(OnToggleMusic);

        _sfxOnImg.GetComponent<Button>().onClick.AddListener(OnToggleSFX);
        _sfxOffImg.GetComponent<Button>().onClick.AddListener(OnToggleSFX);
    }
    private void SyncUI()
    {
        if (AudioManager.Instance == null)
            return;

        var audio = AudioManager.Instance;

        _musicSlider.SetValueWithoutNotify(audio.MusicVolume);
        _sfxSlider.SetValueWithoutNotify(audio.SFXVolume);

        UpdateMusicUI(audio.MusicMuted);
        UpdateSFXUI(audio.SFXMuted);
    }
    public void Show()
    {
        SyncUI();

        _settingPanel.alpha = 0;
        _settingPanel.blocksRaycasts = true;
        _settingPanel.interactable = true;

        _settingPanel.DOFade(1, 0.4f).SetUpdate(true);
    }

    public void Hide()
    {
        _settingPanel.DOFade(0, 0.3f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                _settingPanel.blocksRaycasts = false;
                _settingPanel.interactable = false;
            });
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);

        if (value <= 0.001f)
            UpdateMusicUI(true);
        else
            UpdateMusicUI(false);
    }


    private void OnToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
        SyncUI();
    }

    private void UpdateMusicUI(bool muted)
    {
        _musicOnImg.gameObject.SetActive(!muted);
        _musicOffImg.gameObject.SetActive(muted);
    }

    private void OnSFXSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);

        if (value <= 0.001f)
            UpdateSFXUI(true);
        else 
            UpdateSFXUI(false);
    }

    private void OnToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
        SyncUI();
    }

    private void UpdateSFXUI(bool muted)
    {
        _sfxOnImg.gameObject.SetActive(!muted);
        _sfxOffImg.gameObject.SetActive(muted);
    }

    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
