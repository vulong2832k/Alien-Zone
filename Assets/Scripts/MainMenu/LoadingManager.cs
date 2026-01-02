using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [Header("UI:")]
    [SerializeField] private CanvasGroup _loadingCanvas;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _progressText;

    [Header("Config:")]
    [SerializeField] private float _fadeDuration = 0.3f;

    [Header("FadeImage:")]
    [SerializeField] private CanvasGroup _fadeBlackGroup;
    [SerializeField] private float _fadeBlackDuration = 0.2f;
    [SerializeField] private float _blackHoldTime = 0.8f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _fadeBlackGroup.alpha = 1f;
        _fadeBlackGroup.blocksRaycasts = true;
        _fadeBlackGroup.interactable = true;

        _loadingCanvas.alpha = 0f;
        _loadingCanvas.blocksRaycasts = false;
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return FadeBlack(true);

        yield return new WaitForSecondsRealtime(_blackHoldTime);

        yield return FadeLoading(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            UpdateProgress(op.progress / 0.9f);
            yield return null;
        }

        UpdateProgress(1f);
        yield return new WaitForSecondsRealtime(0.1f);

        op.allowSceneActivation = true;
        yield return null;

        yield return FadeLoading(false);
        yield return FadeBlack(false);
    }

    private IEnumerator FadeLoading(bool show)
    {
        float start = _loadingCanvas.alpha;
        float end = show ? 1f : 0f;
        float time = 0f;

        _loadingCanvas.blocksRaycasts = show;
        _loadingCanvas.interactable = show;

        while (time < _fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            _loadingCanvas.alpha = Mathf.Lerp(start, end, time / _fadeDuration);
            yield return null;
        }

        _loadingCanvas.alpha = end;
    }
    private IEnumerator FadeBlack(bool show)
    {
        float start = _fadeBlackGroup.alpha;
        float end = show ? 1f : 0f;
        float time = 0f;

        _fadeBlackGroup.blocksRaycasts = show;
        _fadeBlackGroup.interactable = show;

        while (time < _fadeBlackDuration)
        {
            time += Time.unscaledDeltaTime;
            _fadeBlackGroup.alpha = Mathf.Lerp(start, end, time / _fadeBlackDuration);
            yield return null;
        }

        _fadeBlackGroup.alpha = end;

        if (!show)
        {
            _fadeBlackGroup.blocksRaycasts = false;
            _fadeBlackGroup.interactable = false;
        }
    }

    private void UpdateProgress(float value)
    {
        if (_progressBar != null)
            _progressBar.value = value;

        if (_progressText != null)
            _progressText.text = $"LOADING... {Mathf.RoundToInt(value * 100f)}%";
    }

    private IEnumerator Fade(bool show)
    {
        float start = _loadingCanvas.alpha;
        float end = show ? 1f : 0f;
        float time = 0f;

        _loadingCanvas.blocksRaycasts = show;
        _loadingCanvas.interactable = show;

        while (time < _fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            _loadingCanvas.alpha = Mathf.Lerp(start, end, time / _fadeDuration);
            yield return null;
        }

        _loadingCanvas.alpha = end;
    }
}
