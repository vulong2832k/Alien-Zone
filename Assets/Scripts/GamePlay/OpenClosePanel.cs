using UnityEngine;

public class OpenClosePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _playerSystemCanvas;
    private bool _isOpen = false;

    private void Start()
    {
        ClosePanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        if (_isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    private void OpenPanel()
    {
        _isOpen = true;

        _playerSystemCanvas.alpha = 1f;
        _playerSystemCanvas.interactable = true;
        _playerSystemCanvas.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        var inventoryUI = _playerSystemCanvas.GetComponentInChildren<InventoryUI>();
        inventoryUI?.Setup(FindAnyObjectByType<InventorySystem>());
    }

    private void ClosePanel()
    {
        _isOpen = false;

        _playerSystemCanvas.alpha = 0f;
        _playerSystemCanvas.interactable = false;
        _playerSystemCanvas.blocksRaycasts = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
