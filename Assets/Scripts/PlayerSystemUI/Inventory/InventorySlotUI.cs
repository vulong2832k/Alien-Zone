using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image _slotImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Sprite _emptySlotSprite;

    public InventorySlot _slot;
    private Transform _originalParent;

    [Header("Drag: ")]
    [SerializeField] private GameObject _dragItemPrefab;
    private RectTransform _dragItemImage;
    private TextMeshProUGUI _dragItemText;

    [SerializeField] private SplitPanelUI _splitPanel;

    public void SetSlot(InventorySlot newSlot)
    {
        _slot = newSlot;
        UpdateUI();
    }
    private void Awake()
    {
        _dragItemImage = _dragItemPrefab.GetComponent<RectTransform>();
        _dragItemText = _dragItemPrefab.GetComponentInChildren<TextMeshProUGUI>();
        _splitPanel = FindAnyObjectByType<SplitPanelUI>(FindObjectsInactive.Include);

        _dragItemPrefab.SetActive(false);
    }
    public void UpdateUI()
    {
        if (_slot == null || _slot.IsEmpty)
        {
            _slotImage.sprite = _emptySlotSprite;
            _amountText.text = "";
            return;
        }

        ItemSO so = _slot.ItemSO;
        _slotImage.sprite = so.icon;
        _slotImage.enabled = true;

        _amountText.text = _slot.amount > 1 ? _slot.amount.ToString() : "";
    }
    public void OnClickSlot()
    {
        if (_slot != null && !_slot.IsEmpty)
        {
            Debug.Log($"Clicked slot: {_slot.item.itemName}, amount: {_slot.amount}");
        }
    }

    //------------------------------------Drag And Drop---------------------------------------------------------//
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_slot == null || _slot.IsEmpty) return;

        _dragItemPrefab.SetActive(true);
        _dragItemImage.position = eventData.position;

        if (_slot.item != null)
        {
            _dragItemImage.GetComponent<Image>().sprite = _slot.item.icon;
        }

        _dragItemText.text = _slot.amount > 1 ? _slot.amount.ToString() : "";

        _slotImage.color = new Color(1, 1, 1, 0.3f);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragItemPrefab.activeSelf)
            _dragItemImage.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragItemPrefab.SetActive(false);
        _slotImage.color = Color.white;
        UpdateUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (dragged == null || dragged == this) return;

        var fromSlot = dragged._slot;
        var toSlot = this._slot;

        //Item giống nhau + có thể stack được
        if (!fromSlot.IsEmpty && !toSlot.IsEmpty && fromSlot.item == toSlot.item && fromSlot.item.isStackable)
        {
            int canMove = Mathf.Min(fromSlot.amount, toSlot.item.maxStack - toSlot.amount);
            toSlot.amount += canMove;
            fromSlot.amount -= canMove;

            if(fromSlot.amount <= 0) fromSlot.Clear();
        }
        //Slot còn trống
        else if (toSlot.IsEmpty && !fromSlot.IsEmpty)
        {
            toSlot.AssignItem(fromSlot.item, fromSlot.amount);
            fromSlot.Clear();
        }
        //Thay đổi vị trí slot nếu không cùng loại item
        else
        {
            var tempItem = toSlot.item;
            var tempAmount = toSlot.amount;

            toSlot.AssignItem(fromSlot.item, fromSlot.amount);
            fromSlot.AssignItem(tempItem, tempAmount);
        }
        dragged.UpdateUI(); this.UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (_slot.item is ArmorSO armorSO)
        {
            TryEquipArmor(armorSO);
        }
    }
    public void OnSplitClicked()
    {
        if (_splitPanel == null)
        {
            return;
        }
        _splitPanel.Show(this);
    }

    public InventorySlot GetSlot()
    {
        return _slot;
    }
    public void TryEquipArmor(ArmorSO armorSO)
    {
        bool equipped = EquipmentSystem.Instance.EquipArmor(armorSO);
        if (!equipped) return;

        _slot.amount--;
        if (_slot.amount <= 0)
            _slot.Clear();

        UpdateUI();
    }
}
