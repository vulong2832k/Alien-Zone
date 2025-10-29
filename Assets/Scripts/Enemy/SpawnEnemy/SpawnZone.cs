using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyLevelSpawn
{
    public LevelEnemy level;
    public int count;
}

[System.Serializable]
public class EnemySpawnInfo
{
    public string poolKey;
    public EnemySO enemyData;
    public EnemyLevelSpawn[] levelDistribution;
}

[RequireComponent(typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private EnemySpawnInfo[] spawnList;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private List<EnemyController> _enemies = new List<EnemyController>();
    public bool IsCleared { get; private set; } = false;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && hasTriggered) return;

        SpawnEnemies();
        hasTriggered = true;
    }

    private void SpawnEnemies()
    {
        IsCleared = false;

        foreach (var info in spawnList)
        {
            foreach (var levelInfo in info.levelDistribution)
            {
                for (int i = 0; i < levelInfo.count; i++)
                {
                    Vector3 pos = GetRandomPointInArea();
                    GameObject enemy = MultiObjectPool.Instance.SpawnFromPool(info.poolKey, pos, Quaternion.identity);
                    if (enemy == null) continue;

                    EnemyController controller = enemy.GetComponent<EnemyController>();
                    if (controller != null)
                    {
                        controller.dataEnemy = info.enemyData;
                        enemy.GetComponent<EnemyStats>().Init(levelInfo.level);

                        _enemies.Add(controller);
                        controller.OnEnemyDie += HandleEnemyDie;
                    }
                }
            }
        }
    }
    private void HandleEnemyDie(EnemyController enemy)
    {
        enemy.OnEnemyDie -= HandleEnemyDie;
        _enemies.Remove(enemy);

        if (_enemies.Count == 0)
        {
            IsCleared = true;
        }
    }

    private Vector3 GetRandomPointInArea()
    {
        Bounds bounds = spawnArea.GetComponent<BoxCollider>().bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y + 5f;

        Vector3 spawnPos = new Vector3(x, y, z);

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return spawnPos;
    }


    private void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnArea.position, spawnArea.localScale);
        }
    }
}
