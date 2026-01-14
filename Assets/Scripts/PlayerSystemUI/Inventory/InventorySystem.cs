using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<InventorySlot> slots = new();
    [SerializeField] private int _slotCount = 20;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        slots.Clear();
        for (int i = 0; i < _slotCount; i++)
            slots.Add(new InventorySlot());
    }

    public int AddItem(ItemSO item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item == item && slot.amount < item.maxStack)
                {
                    int add = Mathf.Min(amount, item.maxStack - slot.amount);
                    slot.amount += add;
                    amount -= add;
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
                int add = Mathf.Min(amount, item.maxStack);
                slot.AssignItem(item, add);
                amount -= add;
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

    public void RemoveItem(ItemSO item, int amount)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item)
            {
                int remove = Mathf.Min(amount, slot.amount);
                slot.amount -= remove;
                amount -= remove;

                if (slot.amount <= 0)
                    slot.Clear();

                if (amount <= 0)
                    break;
            }
        }

        OnInventoryChanged?.Invoke();
    }
    public void ForceRefresh()
    {
        OnInventoryChanged?.Invoke();
    }
}
