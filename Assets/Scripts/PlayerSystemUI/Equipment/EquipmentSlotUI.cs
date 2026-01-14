using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public event Action<ItemSO, int> OnSlotChanged;

    [SerializeField] private ItemType _allowedType;
    public ItemType AllowedType => _allowedType;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _emptySlotSprite;
    [SerializeField] private TextMeshProUGUI _amountText;

    [SerializeField] private int _slotIndex;
    [SerializeField] private WeaponSwitching _weaponSwitching;

    private InventorySlot _slot = new();

    public bool IsEmpty => _slot.IsEmpty;
    public ItemSO GetItem() => _slot.item;

    

    public bool IsArmorSlot =>
        _allowedType == ItemType.Armor ||
        _allowedType == ItemType.HeadArmor;

    private void OnEnable()
    {
        if (_weaponSwitching == null)
            _weaponSwitching = WeaponSwitching.Instance;
    }

    private void Start()
    {
        _icon.sprite = _emptySlotSprite;
        _icon.enabled = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedUI = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (draggedUI == null) return;

        var fromSlot = draggedUI._slot;
        if (fromSlot.IsEmpty) return;

        var item = fromSlot.item;
        if (item == null || item.itemType != _allowedType) return;

        // ===== ARMOR =====
        if (IsArmorSlot)
        {
            var armor = item as ArmorSO;
            if (armor == null) return;

            EquipmentSystem.Instance.Equip(item);

            _slot.AssignItem(item, 1);
            _icon.sprite = item.icon;

            fromSlot.amount--;
            if (fromSlot.amount <= 0)
                fromSlot.Clear();

            draggedUI.UpdateUI();
            InventorySystem.Instance.ForceRefresh();
            return;
        }

        int maxAllowed = GetMaxAllowed(item.itemType);
        int canAdd = maxAllowed - _slot.amount;
        if (canAdd <= 0) return;

        int move = Mathf.Min(fromSlot.amount, canAdd);

        if (_slot.IsEmpty)
            _slot.AssignItem(item, move);
        else if (_slot.item == item)
            _slot.amount += move;
        else
            return;

        _icon.sprite = item.icon;

        fromSlot.amount -= move;
        if (fromSlot.amount <= 0)
            fromSlot.Clear();

        draggedUI.UpdateUI();
        UpdateUI();

        if (_allowedType == ItemType.Weapon && item.gunAttributes != null)
            _weaponSwitching?.SpawnAndEquipWeapon(_slotIndex, item.gunAttributes, true);
    }

    public void Unequip()
    {
        if (_slot.IsEmpty) return;

        InventorySystem.Instance.AddItem(_slot.item, _slot.amount);

        if (IsArmorSlot)
            EquipmentSystem.Instance.Unequip(_slot.item.equipmentSlot);

        if (_allowedType == ItemType.Weapon)
            _weaponSwitching?.SpawnAndEquipWeapon(_slotIndex, null, false);

        _slot.Clear();
        UpdateUI();
    }
    public bool ReduceItem(int value = 1)
    {
        if (_slot == null) return false;

        bool result = _slot.ReduceItem(value);

        UpdateUI();
        return result;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Unequip();
    }

    private void UpdateUI()
    {
        _icon.sprite = _slot.IsEmpty ? _emptySlotSprite : _slot.item.icon;
        _amountText.text = (_slot.amount > 1) ? _slot.amount.ToString() : "";
        OnSlotChanged?.Invoke(_slot.item, _slot.amount);
    }

    private int GetMaxAllowed(ItemType type) => type switch
    {
        ItemType.Weapon => 1,
        ItemType.HeadArmor => 1,
        ItemType.Armor => 1,
        ItemType.Medicine => 10,
        ItemType.Grenade => 15,
        _ => 20
    };
}

