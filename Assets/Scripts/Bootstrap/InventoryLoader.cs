using UnityEngine;

public class InventoryLoader : MonoBehaviour
{
    [SerializeField] private ItemDatabase _itemDatabase;

    private void Start()
    {
        LoadInventory();
    }

    public void LoadInventory()
    {
        var data = PlayerDataManager.Instance;
        var inventory = InventorySystem.Instance;

        if (data == null || inventory == null) return;

        inventory.slots.Clear();

        foreach (var saved in data.GetSavedInventory())
        {
            ItemSO item = _itemDatabase.GetItemById(saved.itemId);
            if (item == null) continue;

            inventory.AddItem(item, saved.amount);
        }

        inventory.ForceRefresh();
    }
}
