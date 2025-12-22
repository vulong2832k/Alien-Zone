using UnityEngine;

public class PauseInputPanel : MonoBehaviour
{
    [SerializeField] private PausePanel _pausePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
                return;

            UIPopupManager.Instance.ShowPausePanel();
        }
    }
}
