using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public event Action<ItemSO, int> OnSlotChanged;

    [SerializeField] private ItemType _allowedType;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _emptySlotSprite;
    [SerializeField] private TextMeshProUGUI _amountText;

    //Gun
    [SerializeField] private int _slotIndex;
    [SerializeField] private WeaponSwitching _weaponSwitching;

    private InventorySlot _slot = new InventorySlot();

    public bool IsEmpty => _slot.IsEmpty;
    public ItemType AllowedType => _allowedType;
    public ItemSO GetItem() => _slot.item;
    private void Start()
    {
        if (_weaponSwitching == null)
        {
            var allSwitchers = FindObjectsByType<WeaponSwitching>(FindObjectsInactive.Include,FindObjectsSortMode.None);

            if (allSwitchers.Length > 0)
            {
                _weaponSwitching = allSwitchers[0];
            }
        }

        _icon.sprite = _emptySlotSprite;
        _icon.enabled = true;
    }
    private int GetMaxAllowed(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon: return 1;
            case ItemType.HeadArmor: return 1;
            case ItemType.Armor: return 1;
            case ItemType.Medicine: return 10;
            case ItemType.Grenade: return 15;
            default: return 99;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlotUI = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (draggedSlotUI == null) return;

        var fromSlot = draggedSlotUI.GetSlot();
        if (fromSlot == null || fromSlot.IsEmpty) return;

        var item = fromSlot.item;
        if (item.itemType != _allowedType) return;

        int maxAllowed = GetMaxAllowed(item.itemType);
        int canAdd = maxAllowed - _slot.amount;
        if (canAdd <= 0) return;

        int moveAmount = Mathf.Min(fromSlot.amount, canAdd);

        if (_slot.IsEmpty)
        {
            _slot.AssignItem(item, moveAmount);
        }
        else if (_slot.item == item)
        {
            _slot.amount += moveAmount;
        }

        _icon.sprite = item.icon;
        _icon.enabled = true;

        fromSlot.amount -= moveAmount;
        if (fromSlot.amount <= 0)
            fromSlot.Clear();
        draggedSlotUI.UpdateUI();

        UpdateAmountText();

        if (_allowedType == ItemType.Weapon && item.gunAttributes != null)
        {
            if (_weaponSwitching != null)
            {
                _weaponSwitching.SpawnAndEquipWeapon(_slotIndex, item.gunAttributes, true);
            }
        }
    }
    public void Unequip()
    {
        if (_slot.IsEmpty) return;

        var inventory = FindAnyObjectByType<InventorySystem>();
        inventory.AddItem(_slot.item, _slot.amount);

        if (_allowedType == ItemType.Weapon && _slot.item.gunAttributes != null)
        {
            _weaponSwitching.SpawnAndEquipWeapon(_slotIndex, null);
        }

        _slot.Clear();
        _icon.sprite = _emptySlotSprite;
        NotifySlotChanged();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Unequip();
        }
    }
    private void UpdateAmountText()
    {
        if (_amountText == null) return;

        if (_slot.IsEmpty || _slot.amount <= 1)
        {
            _amountText.text = "";
        }
        else
        {
            _amountText.text = _slot.amount.ToString();
        }
        NotifySlotChanged();
    }
    public void ReduceItem(int amount)
    {
        if (_slot.IsEmpty) return;

        _slot.amount -= amount;
        if (_slot.amount <= 0)
        {
            _slot.Clear();
            _icon.sprite = _emptySlotSprite;
        }

        UpdateAmountText();
        NotifySlotChanged();
    }
    private void NotifySlotChanged()
    {
        if (_slot.IsEmpty)
            OnSlotChanged?.Invoke(null, 0);
        else
            OnSlotChanged?.Invoke(_slot.item, _slot.amount);
    }
}
