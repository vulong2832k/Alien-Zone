using UnityEngine;
using DG.Tweening;

public class UIPopupManager : MonoBehaviour
{
    public static UIPopupManager Instance;

    [SerializeField] private CanvasGroup _winPanel;
    [SerializeField] private CanvasGroup _losePanel;
    [SerializeField] private CanvasGroup _settingPanel;
    [SerializeField] private CanvasGroup _pausePanel;

    [SerializeField] private float _fadeDuration = 0.6f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideAll();
    }
    private void HideAll()
    {
        SetCanvasGroup(_winPanel, 0);
        SetCanvasGroup(_losePanel, 0);
        SetCanvasGroup(_settingPanel, 0);
        SetCanvasGroup(_pausePanel, 0);
    }
    public void ShowLosePanel()
    {
        FadeIn(_losePanel);
        CursorManager.Instance.UnlockCursor();
    }
    public void ShowWinPanel(GameResultData data)
    {
        Time.timeScale = 0f;

        FadeIn(_winPanel);
        CursorManager.Instance.UnlockCursor();

        var victoryPanel = _winPanel.GetComponent<VictoryPanel>();
        if (victoryPanel != null)
            victoryPanel.SetData(data);
    }

    public void ShowSettingPanel()
    {
        FadeIn(_settingPanel);
        CursorManager.Instance.UnlockCursor();
    }
    public void ShowPausePanel()
    {
        Time.timeScale = 0f;
        FadeIn(_pausePanel);
        CursorManager.Instance.UnlockCursor();
    }
    public void HidePausePanel()
    {
        Time.timeScale = 1f;
        SetCanvasGroup(_pausePanel, 0);
    }
    public void FadeIn(CanvasGroup cg)
    {
        cg.interactable = true;
        cg.blocksRaycasts = true;

        cg.DOKill();
        cg.alpha = 0;

        cg.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad).SetUpdate(true);
    }
    private void SetCanvasGroup(CanvasGroup cg, float alpha)
    {
        if (cg == null) return;

        cg.alpha = alpha;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
