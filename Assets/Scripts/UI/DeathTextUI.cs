using UnityEngine;

public class DeathTextUI : MonoBehaviour
{
    [SerializeField] private GameObject _restartPanel;

    private void Start()
    {
        _restartPanel.SetActive(false);

        var player = FindAnyObjectByType<PlayerController>();
        player.OnPlayerDead += ShowRestartHint;
    }

    private void ShowRestartHint()
    {
        Invoke(nameof(ShowAfterDelay), 3f);
    }

    private void ShowAfterDelay()
    {
        _restartPanel.SetActive(true);
    }
}
