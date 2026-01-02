using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplitPanelUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField _quantityInput;
    [SerializeField] private Button _splitBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private CanvasGroup _canvasGroup;

    private InventorySlotUI _currentSlot;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        _splitBtn.onClick.AddListener(OnSplit);
        _cancelBtn.onClick.AddListener(Hide);
    }

    public void Show(InventorySlotUI slot)
    {
        _currentSlot = slot;
        _quantityInput.text = "";

        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _quantityInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        _quantityInput.Select();
        _quantityInput.ActivateInputField();
    }

    private void Hide()
    {
        _currentSlot = null;

        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void OnSplit()
    {
        if (_currentSlot == null) return;

        if (!int.TryParse(_quantityInput.text, out int amount))
        {
            Hide();
            return;
        }

        var slot = _currentSlot.GetSlot();

        if (amount <= 0 || amount >= slot.amount)
        {
            Hide();
            return;
        }

        slot.amount -= amount;

        var inventory = FindAnyObjectByType<InventorySystem>();
        if (inventory == null)
        {
            slot.amount += amount;
            Hide();
            return;
        }

        InventorySlot empty = null;
        foreach (var s in inventory.slots)
        {
            if (s.IsEmpty)
            {
                empty = s;
                break;
            }
        }

        if (empty == null)
        {
            slot.amount += amount;
            Hide();
            return;
        }

        empty.AssignItem(slot.item, amount);

        inventory.ForceRefresh();
        _currentSlot.UpdateUI();

        Hide();
    }
}
