using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class SceneLootManager : MonoBehaviour
{
    public static SceneLootManager Instance;

    [SerializeField] private EnemyLootConfig[] _lootConfigs;
    private Dictionary<EnemySO, EnemyLootConfig> _lootDict;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _lootDict = new Dictionary<EnemySO, EnemyLootConfig>();

        foreach (var config in _lootConfigs)
        {
            if (config.enemyData != null && !_lootDict.ContainsKey(config.enemyData))
            {
                _lootDict.Add(config.enemyData, config);
            }
        }
    }
    public ItemSO GetLootForEnemy(EnemySO enemy, out int amount)
    {
        amount = 0;

        if (enemy == null || !_lootDict.ContainsKey(enemy)) return null;

        var config = _lootDict[enemy];
        foreach (var entry in config.lootEntries)
        {
            if (Random.value <= entry.dropChange)
            {
                amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                return entry.item;
            }
        }
        return null;
    }
}
