using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanelUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _losePanel;

    [Header("Button: ")]
    [SerializeField] private Button _resetGameBtn;
    [SerializeField] private Button _settingGameBtn;
    [SerializeField] private Button _exitGameBtn;

    private void Awake()
    {
        _resetGameBtn.onClick.AddListener(OnResetClicked);
        _settingGameBtn.onClick.AddListener(OnSettingClicked);
        _exitGameBtn.onClick.AddListener(OnExitClicked);
    }
    private void OnDestroy()
    {
        _resetGameBtn.onClick.RemoveListener(OnResetClicked);
        _settingGameBtn.onClick.RemoveListener(OnSettingClicked);
        _exitGameBtn.onClick.RemoveListener(OnExitClicked);
    }
    private void OnResetClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnSettingClicked()
    {
        UIPopupManager.Instance.ShowSettingPanel();
    }
    private void OnExitClicked()
    {
        Time.timeScale = 1f;
        LoadingManager.Instance.LoadScene("MainMenu");
    }
}
