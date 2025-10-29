using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemSO item;
    public int amount;

    public bool IsEmpty => item == null || amount <= 0;

    public InventorySlot()
    {
        Clear();
    }
    public InventorySlot(ItemSO newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
    }
    public void Clear()
    {
        item = null;
        amount = 0;
    }

    public void AssignItem(ItemSO newItem, int amount)
    {
        if (item == newItem && !IsEmpty)
        {
            this.amount += amount;
        }
        else
        {
            item = newItem;
            this.amount = amount;
        }
    }



}