using UnityEngine;

public class CollectItemsCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private ItemSO _itemCollect;
    [SerializeField] private int _requiredItemCollect;
    private int _collected = 0;

    public void AddItem(ItemSO item)
    {
        if (item == _itemCollect)
        {
            _collected++;
        }
    }

    public void StartCondition()
    {
        _collected = 0;
    }
    public bool IsCompleted()
    {
        return _collected >= _requiredItemCollect;
    }

}
