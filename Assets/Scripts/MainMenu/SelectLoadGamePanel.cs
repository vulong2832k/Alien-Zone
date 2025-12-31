using UnityEngine;

public class SelectLoadGamePanel : MonoBehaviour
{
    [SerializeField] private Transform _contentRoot;
    [SerializeField] private LoadSlotBtn _slotButtonPrefab;

    [SerializeField] private int _maxSlot = 5;

    private void OnEnable()
    {
        if (!IsValid()) return;
        if (PlayerDataManager.Instance == null) return;

        Refresh();
    }
    bool IsValid()
    {
        return _contentRoot != null && _slotButtonPrefab != null;
    }
    public void Refresh()
    {
        Clear();

        var manager = PlayerDataManager.Instance;
        if (manager == null) return;

        for (int i = 0; i < _maxSlot; i++)
        {
            string slotId = $"slot_{i}";
            PlayerData data = manager.LoadSlot(slotId);

            LoadSlotBtn btn = Instantiate(_slotButtonPrefab, _contentRoot);

            if (data == null)
            {
                btn.SetupEmpty(slotId);
            }
            else
            {
                btn.Setup(slotId, data);
            }
        }
    }
    private void Clear()
    {
        foreach (Transform child in _contentRoot)
            Destroy(child.gameObject);
    }
    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
