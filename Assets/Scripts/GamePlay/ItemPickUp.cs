using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public ItemSO itemData;
    public int amount = 1;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public KeyCode pickupKey = KeyCode.C;

    [Header("Attributes")]
    [SerializeField] private float _rotateSpeed = 50f;

    private InventorySystem _playerInventory;
    private Collider _playerCollider;
    private Renderer _renderer;
    private Color _originalColor;
    private bool _isPlayerInRange;

    public void Setup(ItemSO data, int amt)
    {
        itemData = data;
        amount = amt;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
            _originalColor = _renderer.material.color;
    }

    private void Start()
    {
        _playerInventory = SpawnPlayer.PlayerInventory;
        if (_playerInventory == null)
        {
            Debug.LogError("PlayerInventory chưa được gán!");
            return;
        }

        _playerCollider = _playerInventory.GetComponentInChildren<Collider>();
        if (_playerCollider == null)
            Debug.LogError("PlayerCollider không tìm thấy trong children của PlayerInventory!");
    }

    private void Update()
    {
        RotateItemPickUp();
        CheckPickup();
    }

    private void CheckPickup()
    {
        if (_playerInventory == null || _playerCollider == null) return;

        float distance = Vector3.Distance(transform.position, _playerCollider.ClosestPoint(transform.position));
        bool inRange = distance <= pickupRange;

        if (inRange != _isPlayerInRange)
        {
            _isPlayerInRange = inRange;
            if (_renderer != null)
                _renderer.material.color = inRange ? Color.green : _originalColor;
        }

        if (_isPlayerInRange && Input.GetKeyDown(pickupKey))
        {
            int remaining = _playerInventory.AddItem(itemData, amount);

            if (remaining <= 0)
            {
                // Kiểm tra mission
                CollectItemsCondition condition = FindAnyObjectByType<CollectItemsCondition>();
                if (condition != null)
                    condition.AddItem(itemData);

                Destroy(gameObject);
            }
            else
            {
                amount = remaining;
            }
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
