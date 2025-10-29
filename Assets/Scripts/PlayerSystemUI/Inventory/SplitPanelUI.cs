using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SplitPanelUI : MonoBehaviour
{
    [Header("UI References: ")]
    [SerializeField] private TMP_InputField _quantityInput;
    [SerializeField] private Button _splitBtn;
    [SerializeField] private Button _cancelBtn;

    private InventorySlotUI _currentSlot;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void Start()
    {
        _splitBtn.onClick.AddListener(OnSplit);
        _cancelBtn.onClick.AddListener(() => gameObject.SetActive(false));
    }
    public void Show(InventorySlotUI slot)
    {
        _currentSlot = slot;
        _quantityInput.text = "";
        gameObject.SetActive(true);

        _quantityInput.contentType = TMP_InputField.ContentType.IntegerNumber;
        _quantityInput.Select();
        _quantityInput.ActivateInputField();
    }
    private void OnSplit()
    {
        if (_currentSlot == null) return;

        if (int.TryParse(_quantityInput.text, out int amount))
        {
            var slot = _currentSlot.GetSlot();

            if (amount > 0 && amount < slot.amount)
            {
                slot.amount -= amount;

                var inventory = FindAnyObjectByType<InventorySystem>();
                if(inventory == null)
                {
                    slot.amount += amount;
                    gameObject.SetActive(false);
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
                if (empty != null)
                {
                    empty.AssignItem(slot.item, amount);
                }
                else
                {
                    slot.amount += amount;
                    gameObject.SetActive(false);
                    return;
                }
                inventory.ForceRefresh();
                _currentSlot.UpdateUI();
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


}
