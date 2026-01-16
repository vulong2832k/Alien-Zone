using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemSO> items;

    private Dictionary<string, ItemSO> _itemMap;

    public void Init()
    {
        if (_itemMap != null) return;

        _itemMap = new Dictionary<string, ItemSO>();
        foreach (var item in items)
        {
            if (item == null || string.IsNullOrEmpty(item.itemId))
                continue;

            if (!_itemMap.ContainsKey(item.itemId))
                _itemMap.Add(item.itemId, item);
            else
                Debug.LogWarning($"Trùng itemId: {item.itemId}");
        }
    }

    public ItemSO GetItemById(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        Init();
        _itemMap.TryGetValue(id, out var item);
        return item;
    }
}
