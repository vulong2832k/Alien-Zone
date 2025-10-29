using UnityEngine;

[System.Serializable]
public class EnemyLootConfig
{
    public EnemySO enemyData;
    public LootEntry[] lootEntries;
}

[CreateAssetMenu(fileName = "LootTableSO", menuName = "LootItem/LootTableSO")]
public class LootEntry : ScriptableObject
{
    public ItemSO item;
    [Range(0f, 1f)] public float dropChange;
    public int minAmount = 1;
    public int maxAmount = 1;
}
