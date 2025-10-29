using UnityEngine;
using System.Collections.Generic;
using System;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] private int _slotCount;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        slots.Clear();
        for (int i = 0; i < _slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    public int AddItem(ItemSO item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item == item && slot.amount < item.maxStack)
                {
                    int canAdd = Mathf.Min(amount, item.maxStack - slot.amount);
                    slot.amount += canAdd;
                    amount -= canAdd;
                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return 0;
                    }
                }
            }
        }
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int canAdd = Mathf.Min(amount, item.maxStack);
                slot.AssignItem(item, canAdd);
                amount -= canAdd;
                if (amount <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return 0;
                }
            }
        }
        OnInventoryChanged?.Invoke();
        return amount;
    }
    public int GetItemCount(ItemSO item)
    {
        int count = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                count += slot.amount;
            }
        }
        return count;
    }
    public void RemoveItem(ItemSO item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (!slot.IsEmpty && slot.item == item)
            {
                int remove = Mathf.Min(amount, slot.amount);
                slot.amount -= remove;
                amount -= remove;

                if (slot.amount <= 0) slot.Clear();

                if (amount <= 0) break;
            }
        }
        OnInventoryChanged?.Invoke();
    }
    public List<InventorySlot> FindSlots(ItemSO item)
    {
        List<InventorySlot> result = new List<InventorySlot>();
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
                result.Add(slot);
        }
        return result;
    }
    public void ForceRefresh()
    {
        OnInventoryChanged?.Invoke();
    }
}
