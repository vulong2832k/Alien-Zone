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

    private bool _musicOn = true;
    private bool _sfxOn = true;

    private void Awake()
    {
        _exitBtn.onClick.AddListener(Hide);

        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        _musicOnImg.GetComponent<Button>().onClick.AddListener(ToggleMusic);
        _musicOffImg.GetComponent<Button>().onClick.AddListener(ToggleMusic);

        _sfxOnImg.GetComponent<Button>().onClick.AddListener(ToggleSFX);
        _sfxOffImg.GetComponent<Button>().onClick.AddListener(ToggleSFX);

        UpdateMusicUI();
        UpdateSFXUI();
    }

    public void Show()
    {
        _settingPanel.gameObject.SetActive(true);
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
                _settingPanel.gameObject.SetActive(false);
                _settingPanel.blocksRaycasts = false;
                _settingPanel.interactable = false;
            });
    }

    private void ToggleMusic()
    {
        _musicOn = !_musicOn;
        SetMusicVolume(_musicOn ? _musicSlider.value : 0f);
        UpdateMusicUI();
    }

    private void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void UpdateMusicUI()
    {
        _musicOnImg.gameObject.SetActive(_musicOn);
        _musicOffImg.gameObject.SetActive(!_musicOn);
    }

    private void ToggleSFX()
    {
        _sfxOn = !_sfxOn;
        SetSFXVolume(_sfxOn ? _sfxSlider.value : 0f);
        UpdateSFXUI();
    }

    private void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
    private void UpdateSFXUI()
    {
        _sfxOnImg.gameObject.SetActive(_sfxOn);
        _sfxOffImg.gameObject.SetActive(!_sfxOn);
    }
    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
