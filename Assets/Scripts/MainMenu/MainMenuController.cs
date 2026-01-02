using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [System.Serializable]
    public class MenuPanel
    {
        public string panelName;
        public CanvasGroup canvasGroup;
    }

    public static MainMenuController Instance { get; private set; }
    [Header("Default Panel")]
    [SerializeField] private string _defaultPanelName = "MainMenuPanel";

    [Header("Panel:")]
    [SerializeField] private List<MenuPanel> _panelList;

    [Header("Config:")]
    [SerializeField] private float _fadeDuration = 0.3f;

    private CanvasGroup _currentPanel;
    private Coroutine _fadeCoroutine;

    private void OnEnable()
    {
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        foreach (var panel in _panelList)
        {
            SetPanel(panel.canvasGroup, false, instant: true);
        }

        CanvasGroup defaultPanel = GetPanel(_defaultPanelName);
        SetPanel(defaultPanel, true, instant: true);
        _currentPanel = defaultPanel;
    }
    public void ShowPanel(string panelName)
    {
        CanvasGroup target = GetPanel(panelName);
        if (target == null || target == _currentPanel) return;

        StartCoroutine(SwitchPanelRoutine(target));
    }
    private IEnumerator SwitchPanelRoutine(CanvasGroup target)
    {
        if (_currentPanel != null)
            yield return StartCoroutine(FadePanel(_currentPanel, false));

        yield return StartCoroutine(FadePanel(target, true));
        _currentPanel = target;

        if (target.TryGetComponent(out SelectLoadGamePanel loadPanel))
        {
            loadPanel.Refresh();
        }
    }

    private CanvasGroup GetPanel(string panelName)
    {
        foreach (var panel in _panelList)
        {
            if (panel.panelName == panelName)
                return panel.canvasGroup;
        }

        return null;
    }
    private IEnumerator FadePanel(CanvasGroup panel, bool show)
    {
        panel.interactable = show;
        panel.blocksRaycasts = show;

        float start = panel.alpha;
        float end = show ? 1f : 0f;

        float time = 0f;
        while (time < _fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            panel.alpha = Mathf.Lerp(start, end, time / _fadeDuration);
            yield return null;
        }

        panel.alpha = end;
    }


    private void SetPanel(CanvasGroup panel, bool show, bool instant)
    {
        if (panel == null) return;

        panel.alpha = show ? 1f : 0f;
        panel.interactable = show;
        panel.blocksRaycasts = show;
    }

    public void ExitCurrentPanel()
    {
        CanvasGroup defaultPanel = GetPanel(_defaultPanelName);
        if (defaultPanel == null || defaultPanel == _currentPanel) return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(SwitchPanelRoutine(defaultPanel));
    }
    public void ClearAllPlayerDataButton()
    {
        PlayerDataManager.Instance.ClearAllData();

        SelectLoadGamePanel panel = FindFirstObjectByType<SelectLoadGamePanel>();
        if (panel != null && panel.gameObject.activeInHierarchy)
        {
            panel.Refresh();
        }
    }


    public void ExitGameButton()
    {
        Application.Quit();
    }
}
