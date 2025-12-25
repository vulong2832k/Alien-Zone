using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    [SerializeField] private List<EquipmentSlotUI> _slots = new List<EquipmentSlotUI>();

    public List<ArmorInstance> Armors = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public EquipmentSlotUI GetSlot(ItemType type)
    {
        if (_slots == null || _slots.Count == 0)
        {
            return null;
        }

        return _slots.Find(slot => slot != null && slot.AllowedType == type);
    }


    public List<EquipmentSlotUI> GetAllSlots()
    {
        return _slots;
    }
    public void AddArmor(ArmorInstance armor)
    {
        Armors.Add(armor);
    }
}
