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

    public HelmetSO headArmor;
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
                return EquipArmor(item as ArmorSO, null);

            case ItemType.HeadArmor:
                return EquipArmor(null, item as HelmetSO);

            default:
                return false;
        }
    }
    public bool EquipArmor(ArmorSO armor, HelmetSO helmet)
    {
        if (armor != null)
        {
            bodyArmor = armor;
            OnEquipmentChanged?.Invoke();
            return true;
        }

        if (helmet != null)
        {
            headArmor = helmet;
            OnEquipmentChanged?.Invoke();
            return true;
        }

        return false;
    }

    public bool EquipArmor(ArmorSO armor)
    {
        return EquipArmor(armor, null);
    }

    public bool EquipHelmet(HelmetSO helmet)
    {
        return EquipArmor(null, helmet);
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
