[System.Serializable]
public class InventorySlot
{
    public ItemSO item;
    public int amount;
    public ArmorInstance armorInstance;

    public bool IsEmpty => item == null && armorInstance == null;

    public ItemSO ItemSO
    {
        get
        {
            if (item != null) return item;
            if (armorInstance != null) return armorInstance.itemSO;
            return null;
        }
    }

    public int Amount => IsEmpty ? 0 : amount;

    public void AssignItem(ItemSO newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
        armorInstance = null;
    }

    public void AssignArmor(ArmorInstance armor)
    {
        armorInstance = armor;
        item = null;
        amount = 1;
    }

    public void Clear()
    {
        item = null;
        armorInstance = null;
        amount = 0;
    }
}
