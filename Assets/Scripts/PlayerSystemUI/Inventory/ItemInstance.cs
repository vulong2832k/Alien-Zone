[System.Serializable]
public abstract class ItemInstance
{
    public ItemSO itemSO;

    public virtual bool IsStackable => itemSO.isStackable;
    public virtual int MaxStack => itemSO.maxStack;
}
