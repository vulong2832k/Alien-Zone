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
    private Transform _playerTransform;
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
        _playerInventory = InventorySystem.Instance;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player (tag = Player)");
            return;
        }

        _playerTransform = player.transform;
    }

    private void Update()
    {
        RotateItemPickUp();
        CheckPickup();
    }

    private void CheckPickup()
    {
        if (_playerTransform == null) return;

        float distance = Vector3.Distance(
            transform.position,
            _playerTransform.position
        );

        if (distance > pickupRange) return;

        if (!Input.GetKeyDown(pickupKey)) return;

        int remaining = _playerInventory.AddItem(itemData, amount);
        int pickedAmount = amount - Mathf.Max(remaining, 0);

        if (pickedAmount > 0)
        {
            LootChatUI.Instance?.AddMessage($"+ {itemData.itemName} x{pickedAmount}");
        }

        if (remaining <= 0)
        {
            CollectItemsCondition condition = FindAnyObjectByType<CollectItemsCondition>();
            condition?.AddItem(itemData);

            Destroy(gameObject);
        }
        else
        {
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
