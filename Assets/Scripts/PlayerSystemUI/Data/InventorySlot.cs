[System.Serializable]
public class InventorySlot
{
    public ItemSO item;
    public int amount;

    public bool IsEmpty => item == null || amount <= 0;

    public ItemSO ItemSO => item;

    public void AssignItem(ItemSO newItem, int newAmount)
    {
        item = newItem;
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
        item = null;
        amount = 0;
    }
}
