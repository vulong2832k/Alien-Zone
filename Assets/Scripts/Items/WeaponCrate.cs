using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrateItemEntry
{
    public ItemSO item;
    public int amount = 1;
}
public class WeaponCrate : MonoBehaviour, IInteractable
{
    private Animator _animator;

    [Header("Mission Crate Settings")]
    [SerializeField] private bool _isMissionCrate = false;
    [SerializeField] private MonoBehaviour _winConditionRef;

    [Header("Reward Items")]
    [SerializeField] private List<CrateItemEntry> _rewards = new List<CrateItemEntry>();

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

        GameManager.Instance.AddChestLoot();

        _animator.SetBool("Open", true);
        StartCoroutine(AutoClose());

        // Add ALL items in list
        foreach (var entry in _rewards)
        {
            if (entry.item == null || entry.amount <= 0)
                continue;

            SpawnPlayer.PlayerInventory.AddItem(entry.item, entry.amount);

            if (LootChatUI.Instance != null)
            {
                LootChatUI.Instance.AddMessage($"+ {entry.item.itemName} x{entry.amount}");
            }
        }

        // Mission
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
