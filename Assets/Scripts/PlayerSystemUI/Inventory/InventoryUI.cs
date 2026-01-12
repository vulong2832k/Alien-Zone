using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySystem _inventory;
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private Transform _slotParent;

    private InventorySlotUI[] _slotUIs;
    private bool _isInitialized = false;

    private void Awake()
    {
        if (_inventory == null)
            _inventory = FindAnyObjectByType<InventorySystem>();
    }

    private void OnEnable()
    {
        if (_inventory == null) return;

        if (!_isInitialized)
            Setup(_inventory);

        _inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        if (_inventory != null)
            _inventory.OnInventoryChanged -= RefreshUI;
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
            ui.SetSlot(_inventory.slots[i]);
            _slotUIs[i] = ui;
        }

        _isInitialized = true;
    }

    private void RefreshUI()
    {
        if (_slotUIs == null) return;

        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].UpdateUI();
        }
    }
}
