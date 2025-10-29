using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public ItemSO itemData;
    public int amount = 1;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public KeyCode pickupKey = KeyCode.C;

    private InventorySystem _playerInventory;
    private Transform _player;
    private Renderer _renderer;
    private Color _originalColor;
    private bool _isPlayerInRange;

    [Header("Attributes: ")]
    [SerializeField] private float _rotateSpeed;

    public void Setup(ItemSO data, int amt)
    {
        itemData = data;
        amount = amt;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }
    }
    private void Start()
    {
        _playerInventory = SpawnPlayer.PlayerInventory;
        if (_playerInventory != null)
            _player = _playerInventory.transform;
    }

    private void Update()
    {
        PickUp();
        RotateItemPickUp();
    }
    private void PickUp()
    {
        if (_player == null || _playerInventory == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        bool inRange = distance <= pickupRange;

        if (inRange != _isPlayerInRange)
        {
            _isPlayerInRange = inRange;
            if (_renderer != null)
            {
                _renderer.material.color = inRange ? Color.green : _originalColor;
            }
        }

        if (_isPlayerInRange && Input.GetKeyDown(pickupKey))
        {
            int remaining = _playerInventory.AddItem(itemData, amount);

            if (remaining <= 0)
            {
                CollectItemsCondition condition = FindAnyObjectByType<CollectItemsCondition>();
                if (condition != null)
                {
                    condition.AddItem(itemData);
                }

                Destroy(gameObject);
            }
                
            else
                amount = remaining;
        }
    }
    private void RotateItemPickUp()
    {
        transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
