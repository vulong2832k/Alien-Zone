[System.Serializable]
public class InventorySlot
{
    public ItemSO itemName;
    public int amount;

    public bool IsEmpty => itemName == null || amount <= 0;

    public ItemSO ItemSO => itemName;

    public void AssignItem(ItemSO newItem, int newAmount)
    {
        itemName = newItem;
        amount = newAmount;
    }
    public bool ReduceItem(int value = 1)
    {
        if (IsEmpty) return false;

        amount -= value;

        if (amount <= 0)
            Clear();

        return true;
    }
    public void Clear()
    {
        itemName = null;
        amount = 0;
    }
}
