using System.Collections;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeUI;
    [SerializeField] private float _delayBeforeFade = 0.2f;
    [SerializeField] private float _fadeDuration = 0.8f;
    [SerializeField] private float _holdWhiteTime = 1f;

    private bool _isActive;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;

        _fadeUI.alpha = 0f;
        _fadeUI.gameObject.SetActive(false);
    }

    public void ActivateExitZone()
    {
        _isActive = true;
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive || !other.CompareTag("Player")) return;

        _isActive = false;
        GetComponent<Collider>().enabled = false;

        StartCoroutine(ExitSequence());
    }

    private IEnumerator ExitSequence()
    {
        yield return new WaitForSeconds(_delayBeforeFade);

        Time.timeScale = 0.2f;

        _fadeUI.gameObject.SetActive(true);
        _fadeUI.blocksRaycasts = true;
        _fadeUI.interactable = false;

        float t = 0f;
        while (t < _fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            _fadeUI.alpha = t / _fadeDuration;
            yield return null;
        }

        _fadeUI.alpha = 1f;

        yield return new WaitForSecondsRealtime(_holdWhiteTime);

        t = 0f;
        float fadeOutDuration = 0.4f;
        while (t < fadeOutDuration)
        {
            t += Time.unscaledDeltaTime;
            _fadeUI.alpha = 1f - (t / fadeOutDuration);
            yield return null;
        }

        _fadeUI.alpha = 0f;
        _fadeUI.gameObject.SetActive(false);

        Time.timeScale = 1f;

        GameManager.Instance.OnExitZoneCompleted();

        _fadeUI.blocksRaycasts = false;
    }

}
