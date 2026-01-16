using UnityEngine;

public class ItemDatabaseHolder : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    [SerializeField] private ItemDatabase database;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = database;
        Instance.Init();

        DontDestroyOnLoad(gameObject);
    }
}
