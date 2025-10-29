using UnityEngine;
using UnityEngine.InputSystem;

public class OpenClosePanel : MonoBehaviour
{
    [SerializeField] private GameObject _playerSystem;
    private bool _isOpen = false;

    private void Start()
    {
        _playerSystem.SetActive(false);
    }

    void Update()
    {
        PanelControlWithKeyBoard();
    }

    private void PanelControlWithKeyBoard()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    switch (key)
                    {
                        case KeyCode.B:
                            OpenClosePlayerSystem();
                            break;


                        default:
                            break;
                    }
                }
            }
        }
    }

    private void OpenClosePlayerSystem()
    {
        _isOpen = !_isOpen;

        _playerSystem.SetActive(_isOpen);

        if (_isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            var inventoryUI = _playerSystem.GetComponentInChildren<InventoryUI>();
            inventoryUI?.Setup(FindAnyObjectByType<InventorySystem>());
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
}
