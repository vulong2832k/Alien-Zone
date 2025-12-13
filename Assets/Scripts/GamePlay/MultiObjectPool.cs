using UnityEngine;
using System.Collections.Generic;

public class MultiObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string key;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private List<Pool> _pools;
    private Dictionary<string, Queue<GameObject>> _poolDict;

    public static MultiObjectPool Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }

            _poolDict.Add(pool.key, objectPool);
        }
    }

    public GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (!_poolDict.ContainsKey(key))
        {
            Debug.LogError($"POOL NOT FOUND: {key}");
            return null;
        }

        // Nếu queue rỗng thì tạo thêm
        if (_poolDict[key].Count == 0)
        {
            Debug.LogWarning($"Pool '{key}' hết object → Auto Expand");
            var pool = _pools.Find(x => x.key == key);
            if (pool != null)
            {
                GameObject newObj = Instantiate(pool.prefab);
                newObj.SetActive(false);
                newObj.transform.SetParent(transform);
                _poolDict[key].Enqueue(newObj);
            }
        }

        GameObject obj = _poolDict[key].Dequeue();
        obj.SetActive(false);

        var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        obj.transform.SetPositionAndRotation(position, rotation);

        if (agent != null)
            agent.Warp(position);

        obj.SetActive(true);

        if (agent != null)
            agent.enabled = true;

        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        if (!_poolDict.ContainsKey(key))
        {
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);

        _poolDict[key].Enqueue(obj);
    }
}
