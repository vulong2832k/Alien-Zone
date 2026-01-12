[System.Serializable]
public class NormalItemInstance : ItemInstance
{
    public int amount;

    public NormalItemInstance(ItemSO so, int amount)
    {
        itemSO = so;
        this.amount = amount;
    }
}
