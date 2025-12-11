using System.Collections;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeUI;

    private bool _isActive = false;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        fadeUI.alpha = 0;
    }

    public void ActivateExitZone()
    {
        _isActive = true;
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive && other.CompareTag("Player"))
        {
            StartCoroutine(ExitEffect());
        }
    }

    private IEnumerator ExitEffect()
    {
        Time.timeScale = 0.2f;

        for (float t = 0; t < 1; t += Time.unscaledDeltaTime * 2f)
        {
            fadeUI.alpha = t;
            yield return null;
        }

        Time.timeScale = 1f;

        Debug.Log("END LEVEL!");
    }
}
