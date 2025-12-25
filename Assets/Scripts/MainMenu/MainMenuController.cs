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

    [Header("Default Panel")]
    [SerializeField] private string _defaultPanelName = "MainMenuPanel";

    [Header("Panel:")]
    [SerializeField] private List<MenuPanel> _panelList;

    [Header("Config:")]
    [SerializeField] private float _fadeDuration = 0.3f;

    private CanvasGroup _currentPanel;

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
        StopAllCoroutines();

        CanvasGroup target = GetPanel(panelName);
        if (target == null || target == _currentPanel) return;

        if (_currentPanel != null)
        {
            StartCoroutine(FadePanel(_currentPanel, false));
        }

        StartCoroutine(FadePanel(target, true));
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
        float start = panel.alpha;
        float end = show ? 1f : 0f;

        if (show)
        {
            panel.gameObject.SetActive(true);
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }

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
            panel.interactable = false;
            panel.blocksRaycasts = false;
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
    public void ExitGameButton()
    {
        Application.Quit();
    }
}
