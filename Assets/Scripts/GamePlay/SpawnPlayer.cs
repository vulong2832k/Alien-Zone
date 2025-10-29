using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPos;

    [Header("References: ")]
    public static InventorySystem PlayerInventory { get; private set; }

    private void Awake()
    {
        Spawn();
    }
    private void Spawn()
    {
        if (_spawnPos == null || _playerPrefab == null) return;

        var player = Instantiate(_playerPrefab, _spawnPos.position, Quaternion.identity);

        var inventory = player.GetComponent<InventorySystem>();
        PlayerInventory = inventory;
        var ui = FindAnyObjectByType<InventoryUI>();
        if (inventory != null && ui != null)
        {
            ui.Setup(inventory);
        }
    }
}
