using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private EnemySO enemyData;

    public void DropLoot()
    {
        if (SceneLootManager.Instance == null) return;

        ItemSO loot = SceneLootManager.Instance.GetLootForEnemy(enemyData, out int amount);
        if (loot == null || loot.worldPrefab == null) return;

        Vector3 spawnPos = transform.position;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 10f))
        {
            spawnPos = hit.point;
        }

        GameObject drop = Instantiate(loot.worldPrefab, transform.position, Quaternion.identity);

        var pickup = drop.GetComponent<ItemPickup>();
        if (pickup == null) return;

        pickup.Setup(loot, amount);
    }
}
