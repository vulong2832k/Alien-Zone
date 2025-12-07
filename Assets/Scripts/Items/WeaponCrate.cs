using System.Collections;
using UnityEngine;

public class WeaponCrate : MonoBehaviour, IInteractable
{
    private Animator _animator;

    [Header("Mission Crate Settings")]
    [SerializeField] private bool _isMissionCrate = false;
    [SerializeField] private MonoBehaviour _winConditionRef;

    [Header("Item Settings")]
    [SerializeField] private ItemSO _itemReward;
    [SerializeField] private int _amount = 1;

    private IWinCondition _winCondition;
    private bool _opened = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_isMissionCrate && _winConditionRef != null)
            _winCondition = _winConditionRef as IWinCondition;
        else if (_isMissionCrate)
            _winCondition = FindFirstObjectByType<FindSecretsCondition>();
    }

    public void Interact(PlayerController player)
    {
        if (_opened) return;

        _opened = true;
        _animator.SetBool("Open", true);
        StartCoroutine(AutoClose());

        // 1. Add Item to Player Inventory (nếu có item)
        if (_itemReward != null)
        {
            //player.Inventory.AddItem(_itemReward, _amount); Bug nè. fix sau nha
            Debug.Log($"Player nhận item: {_itemReward.name} x{_amount}");
        }

        // 2. Update Mission Condition nếu đây là thùng nhiệm vụ
        if (_isMissionCrate && _winCondition != null)
        {
            if (_winCondition is FindSecretsCondition secretCondition)
            {
                secretCondition.RegisterSecretFound();
                Debug.Log("Mission secret registered!");
            }
        }
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(4f);
        _animator.SetBool("Open", false);
    }
}
