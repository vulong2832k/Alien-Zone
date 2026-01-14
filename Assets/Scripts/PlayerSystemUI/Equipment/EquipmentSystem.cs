using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public ArmorSO headArmor;
    public ArmorSO bodyArmor;

    [Header("Equipment Slots")]
    [SerializeField] private List<EquipmentSlotUI> _slots = new();

    public event Action OnEquipmentChanged;

    public IReadOnlyList<EquipmentSlotUI> GetAllSlots()
    {
        return _slots;
    }
    public bool Equip(ItemSO item)
    {
        if (item == null) return false;

        switch (item.itemType)
        {
            case ItemType.Armor:
            case ItemType.HeadArmor:
                return EquipArmor(item as ArmorSO);

            default:
                return false;
        }
    }

    public bool EquipArmor(ArmorSO armor)
    {
        if (armor == null) return false;

        switch (armor.equipmentSlot)
        {
            case EquipmentSlot.Head:
                headArmor = armor;
                break;
            case EquipmentSlot.Body:
                bodyArmor = armor;
                break;
            default:
                return false;
        }

        OnEquipmentChanged?.Invoke();
        return true;
    }
    public EquipmentSlotUI GetSlot(ItemType type)
    {
        foreach (var slot in _slots)
        {
            if (slot != null && slot.AllowedType == type)
                return slot;
        }
        return null;
    }
    public void Unequip(EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.Head)
            headArmor = null;
        else if (slot == EquipmentSlot.Body)
            bodyArmor = null;

        OnEquipmentChanged?.Invoke();
    }
}
