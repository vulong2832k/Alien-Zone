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
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    private void Start()
    {
        LoadFromSave();
    }
    private void LoadFromSave()
    {
        if (PlayerDataManager.Instance == null) return;
        var data = PlayerDataManager.Instance.CurrentData;
        if (data == null || data.inventoryItems == null) return;

        var itemDB = ItemDatabaseHolder.Instance;
        if (itemDB == null)
        {
            Debug.LogError("ItemDatabaseLoader chưa tồn tại!");
            return;
        }

        foreach (var save in data.inventoryItems)
        {
            ItemSO item = itemDB.GetItemById(save.itemId);
            if (item == null)
            {
                Debug.LogWarning($"Không tìm thấy itemId: {save.itemId}");
                continue;
            }

            AddItem(item, save.amount);
        }

        OnInventoryChanged?.Invoke();
    }


    public int AddItem(ItemSO item, int amount)
    {
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.itemName == item && slot.amount < item.maxStack)
                {
                    int spaceLeft = item.maxStack - slot.amount;
                    int add = Mathf.Min(amount, spaceLeft);

                    if (add <= 0) continue;

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
                int add = item.isStackable
                    ? Mathf.Min(amount, item.maxStack)
                    : 1;

                if (add <= 0) continue;

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
            if (!slot.IsEmpty && slot.itemName == item)
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
    public List<InventorySlot> GetAllSlots()
    {
        return slots;
    }
}
