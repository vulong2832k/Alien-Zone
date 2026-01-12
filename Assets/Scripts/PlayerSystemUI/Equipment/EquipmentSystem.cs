using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    [SerializeField] private List<EquipmentSlotUI> _slots = new();

    private ArmorInstance _bodyArmor;
    private ArmorInstance _headArmor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public EquipmentSlotUI GetSlot(ItemType type)
    {
        return _slots.Find(s => s != null && s.AllowedType == type);
    }

    public bool EquipArmor(ArmorInstance armor)
    {
        if (armor == null) return false;

        switch (armor.itemSO.itemType)
        {
            case ItemType.Armor:
                SwapArmor(ref _bodyArmor, armor);
                break;

            case ItemType.HeadArmor:
                SwapArmor(ref _headArmor, armor);
                break;

            default:
                return false;
        }

        ApplyStats();
        return true;
    }

    private void SwapArmor(ref ArmorInstance current, ArmorInstance next)
    {
        if (current != null)
        {
            InventorySystem.Instance.AddArmor(current);
        }
        current = next;
    }


    public void Unequip(ItemType type)
    {
        switch (type)
        {
            case ItemType.Armor:
                ReturnToInventory(ref _bodyArmor);
                break;

            case ItemType.HeadArmor:
                ReturnToInventory(ref _headArmor);
                break;
        }

        ApplyStats();
    }

    private void ReturnToInventory(ref ArmorInstance armor)
    {
        if (armor == null) return;

        InventorySystem.Instance.AddItem((ArmorSO)armor.itemSO, 1);
        armor = null;
    }


    public ArmorInstance GetArmor(ItemType type)
    {
        return type == ItemType.Armor ? _bodyArmor : _headArmor;
    }

    public List<EquipmentSlotUI> GetAllSlots()
    {
        return _slots;
    }

    private void ApplyStats()
    {
        PlayerStats.Instance.RecalculateStats(_bodyArmor, _headArmor);
    }
}
