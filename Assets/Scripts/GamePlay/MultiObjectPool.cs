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
            return null;
        }

        var objectToSpawn = _poolDict[key].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        _poolDict[key].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}
