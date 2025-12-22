using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pausePanel;

    [Header("Buttons: ")]
    [SerializeField] private Button _resetBtn;
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _exitBtn;

    private void Awake()
    {
        _resetBtn.onClick.AddListener(OnResetClicked);
        _resumeBtn.onClick.AddListener(OnResumeClicked);
        _settingBtn.onClick.AddListener(OnSettingClicked);
        _exitBtn.onClick.AddListener(OnExitClicked);
    }
    private void OnDestroy()
    {
        _resetBtn.onClick.RemoveListener(OnResetClicked);
        _resumeBtn.onClick.RemoveListener(OnResumeClicked);
        _settingBtn.onClick.RemoveListener(OnSettingClicked);
        _exitBtn.onClick.RemoveListener(OnExitClicked);
    }
    private void OnExitClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSettingClicked()
    {
        UIPopupManager.Instance.ShowSettingPanel();
    }

    private void OnResumeClicked()
    {
        Time.timeScale = 1f;
        UIPopupManager.Instance.HidePausePanel();
        CursorManager.Instance.LockCursor();
    }

    private void OnResetClicked()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}

