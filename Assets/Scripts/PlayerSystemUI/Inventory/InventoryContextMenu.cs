using UnityEngine;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    public static InventoryContextMenu Instance;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button splitBtn;
    [SerializeField] private Button dropBtn;

    private InventorySlotUI _currentSlot;
    private Canvas _canvas;

    private Camera UICamera =>
        _canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : _canvas.worldCamera;

    private void Awake()
    {
        Instance = this;

        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
            _canvas = FindAnyObjectByType<Canvas>();

        Hide();
    }

    private void Update()
    {
        if (!_canvasGroup.blocksRaycasts) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                transform as RectTransform,
                Input.mousePosition,
                UICamera))
            {
                Hide();
            }
        }
    }

    public void Show(InventorySlotUI slotUI, Vector3 screenPos)
    {
        gameObject.SetActive(true);

        _currentSlot = slotUI;
        if (_canvas == null) return;

        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            UICamera,
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

        slot.Clear();
        _currentSlot.UpdateUI();
        Hide();
    }
}
