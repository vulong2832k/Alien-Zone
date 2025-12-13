using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public event Action<ItemSO, int> OnSlotChanged;

    [SerializeField] private ItemType _allowedType;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _emptySlotSprite;

    [SerializeField] private TextMeshProUGUI _amountText;

    // Gun
    [SerializeField] private int _slotIndex;
    [SerializeField] private WeaponSwitching _weaponSwitching;

    private InventorySlot _slot = new InventorySlot();

    public bool IsEmpty => _slot.IsEmpty;
    public ItemType AllowedType => _allowedType;
    public ItemSO GetItem() => _slot.item;

    private void Awake()
    {
        if (_weaponSwitching == null)
        {
            var allSwitchers = FindObjectsByType<WeaponSwitching>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (allSwitchers.Length > 0)
                _weaponSwitching = allSwitchers[0];
            else
                Debug.LogError("Không tìm thấy WeaponSwitching trong scene!");
        }
    }

    private void Start()
    {
        if (_weaponSwitching == null)
        {
            var allSwitchers = FindObjectsByType<WeaponSwitching>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (allSwitchers.Length > 0)
                _weaponSwitching = allSwitchers[0];
        }

        _icon.sprite = _emptySlotSprite;
        _icon.enabled = true;
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
            _slot.AssignItem(item, moveAmount);
        else if (_slot.item == item)
            _slot.amount += moveAmount;

        _icon.sprite = item.icon;
        _icon.enabled = true;

        fromSlot.amount -= moveAmount;
        if (fromSlot.amount <= 0)
            fromSlot.Clear();
        draggedSlotUI.UpdateUI();

        UpdateAmountText();
        NotifySlotChanged();

        if (_allowedType == ItemType.Weapon && item.gunAttributes != null)
        {
            if (_weaponSwitching != null)
            {
                GunController newGun = _weaponSwitching.SpawnAndEquipWeapon(_slotIndex, item.gunAttributes, true);
            }
        }
    }

    public void Unequip()
    {
        if (_slot.IsEmpty) return;

        var inventory = FindAnyObjectByType<InventorySystem>();
        inventory.AddItem(_slot.item, _slot.amount);

        if (_allowedType == ItemType.Weapon && _slot.item.gunAttributes != null && _weaponSwitching != null)
        {
            _weaponSwitching.SpawnAndEquipWeapon(_slotIndex, null, false);
        }

        _slot.Clear();
        _icon.sprite = _emptySlotSprite;
        UpdateAmountText();
        NotifySlotChanged();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Unequip();
    }

    private void UpdateAmountText()
    {
        if (_amountText == null) return;

        _amountText.text = (_slot.IsEmpty || _slot.amount <= 1) ? "" : _slot.amount.ToString();
    }

    private int GetMaxAllowed(ItemType type)
    {
        return type switch
        {
            ItemType.Weapon => 1,
            ItemType.HeadArmor => 1,
            ItemType.Armor => 1,
            ItemType.Medicine => 10,
            ItemType.Grenade => 15,
            _ => 99,
        };
    }

    public void ReduceItem(int amount)
    {
        if (_slot.IsEmpty) return;

        _slot.amount -= amount;
        if (_slot.amount <= 0)
        {
            _slot.Clear();
            _icon.sprite = _emptySlotSprite;

            if (_allowedType == ItemType.Weapon && _weaponSwitching != null)
                _weaponSwitching.SpawnAndEquipWeapon(_slotIndex, null, false);
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
