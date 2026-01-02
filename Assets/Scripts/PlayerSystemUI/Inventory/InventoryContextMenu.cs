using UnityEngine;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button splitBtn;
    [SerializeField] private Button dropBtn;

    private InventorySlotUI _currentSlot;
    private Canvas _canvas;

    public static InventoryContextMenu Instance;

    private void Awake()
    {
        Instance = this;
        Hide();

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            _canvas = FindAnyObjectByType<Canvas>();
    }

    private void Update()
    {
        if (!_canvasGroup.blocksRaycasts) return;

        // Click ra ngoài menu => đóng
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera))
            {
                Hide();
            }
        }
    }

    public void Show(InventorySlotUI slotUI, Vector3 screenPos)
    {
        _currentSlot = slotUI;
        if (_canvas == null) return;

        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        transform.SetParent(_canvas.transform, false);
        (transform as RectTransform).localPosition = localPoint;

        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        splitBtn.onClick.RemoveAllListeners();
        dropBtn.onClick.RemoveAllListeners();

        splitBtn.onClick.AddListener(SplitStack);
        dropBtn.onClick.AddListener(DropItem);
    }

    public void Hide()
    {
        _currentSlot = null;
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void SplitStack()
    {
        if (_currentSlot == null) return;

        var slot = _currentSlot.GetSlot();
        if (slot == null || slot.IsEmpty || slot.amount < 2) return;

        _currentSlot.OnSplitClicked();
        Hide();
    }

    private void DropItem()
    {
        if (_currentSlot == null) return;

        var slot = _currentSlot.GetSlot();
        if (slot == null || slot.IsEmpty) return;

        ItemSO item = slot.item;
        int amount = slot.amount;

        if (item != null && item.worldPrefab != null)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 dropPos = player.position + player.forward * 1f;

            GameObject drop = Instantiate(item.worldPrefab, dropPos, Quaternion.identity);

            var pickup = drop.GetComponent<ItemPickup>();
            pickup?.Setup(item, amount);
        }

        slot.Clear();
        _currentSlot.UpdateUI();
        Hide();
    }
}
