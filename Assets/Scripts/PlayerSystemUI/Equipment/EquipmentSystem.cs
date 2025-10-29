using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    [SerializeField] private List<EquipmentSlotUI> _slots = new List<EquipmentSlotUI>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public EquipmentSlotUI GetSlot(ItemType type)
    {
        return _slots.Find(slot => slot.AllowedType == type);
    }

    public List<EquipmentSlotUI> GetAllSlots()
    {
        return _slots;
    }
}
