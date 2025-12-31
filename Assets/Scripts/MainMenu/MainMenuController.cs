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

    [SerializeField] private NewGamePanel _newGamePanel;

    [Header("Config:")]
    [SerializeField] private float _fadeDuration = 0.3f;

    private CanvasGroup _currentPanel;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
    public void ShowNewGamePanel(string slotId)
    {
        _newGamePanel.SetSlot(slotId);
        ShowPanel("NamePlayerPanel");
    }
    private IEnumerator SwitchPanelRoutine(CanvasGroup target)
    {
        if (_currentPanel != null)
            yield return StartCoroutine(FadePanel(_currentPanel, false));

        yield return StartCoroutine(FadePanel(target, true));
        _currentPanel = target;
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
        if (show)
        {
            panel.gameObject.SetActive(true);
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }
        else
        {
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }

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

        if (!show)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private void SetPanel(CanvasGroup panel, bool show, bool instant)
    {
        if (panel == null) return;

        panel.alpha = show ? 1f : 0f;
        panel.interactable = show;
        panel.blocksRaycasts = show;
        panel.gameObject.SetActive(show);
    }
    public void ExitCurrentPanel()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        if (_currentPanel != null)
            _fadeCoroutine = StartCoroutine(FadePanel(_currentPanel, false));

        CanvasGroup defaultPanel = GetPanel(_defaultPanelName);
        if (defaultPanel != null)
        {
            _fadeCoroutine = StartCoroutine(FadePanel(defaultPanel, true));
            _currentPanel = defaultPanel;
        }
    }
    public void ExitGameButton()
    {
        Application.Quit();
    }
}
