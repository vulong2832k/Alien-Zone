using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySystem _inventory;
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private Transform _slotParent;

    private InventorySlotUI[] _slotUIs;

    private void Awake()
    {
        if (_inventory == null)
            _inventory = FindAnyObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        if (_inventory != null)
        {
            Setup(_inventory);
            _inventory.OnInventoryChanged += RefreshUI;
        }
    }
    private void OnDisable()
    {
        if (_inventory != null)
        {
            _inventory.OnInventoryChanged -= RefreshUI;
        }
    }
    public void Setup(InventorySystem inventory)
    {
        _inventory = inventory;

        foreach (Transform child in _slotParent)
            Destroy(child.gameObject);

        _slotUIs = new InventorySlotUI[_inventory.slots.Count];
        for (int i = 0; i < _inventory.slots.Count; i++)
        {
            var ui = Instantiate(_slotPrefab, _slotParent);
            _slotUIs[i] = ui;
            ui.SetSlot(_inventory.slots[i]);
        }
        RefreshUI();
    }
    private void RefreshUI()
    {
        if (_slotUIs == null) return;

        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i]?.UpdateUI();
        }
    }
}
