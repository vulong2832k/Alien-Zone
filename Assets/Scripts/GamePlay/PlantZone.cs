using UnityEngine;

public class PlantZone : MonoBehaviour
{
    [Header("C4 Settings: ")]
    [SerializeField] private GameObject _c4Prefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemSO _requiredItem;
    public bool IsPlanted { get; private set; } = false;

    public void Plant()
    {
        if (IsPlanted) return;

        IsPlanted = true;

        Vector3 pos = _spawnPoint ? _spawnPoint.position : transform.position;
        Quaternion rot = _spawnPoint ? _spawnPoint.rotation : transform.rotation;

        GameObject bombObj = Instantiate(_c4Prefab, pos, rot);
        C4Controller bomb = bombObj.GetComponent<C4Controller>();
        if (bomb != null) bomb.Plant();

        gameObject.SetActive(false);
    }

    private bool HasItem(InventorySystem inventory, ItemSO item)
    {
        foreach (var slot in inventory.slots)
        {
            if (!slot.IsEmpty && slot.item == item && slot.amount > 0)
            {
                return true;
            }
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InventorySystem inventory = SpawnPlayer.PlayerInventory;

        PlantBomCondition condition = FindAnyObjectByType<PlantBomCondition>();
        if (condition != null)
        {
            condition.TryPlant(this, inventory);
        }
        else
        {
            if (HasItem(inventory, _requiredItem))
            {
                inventory.RemoveItem(_requiredItem, 1);
                Plant();
            }
        }
    }
}
