using System.Collections.Generic;
using UnityEngine;

public class PlantBomCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private List<PlantZone> _plantZones;
    [SerializeField] private ItemSO _requiredItem;

    private int _plantedCount = 0;
    private int _totalZones;

    public void StartCondition()
    {
        _plantedCount = 0;
        _totalZones = _plantZones.Count;
    }

    public bool IsCompleted()
    {
        return _plantedCount >= _totalZones;
    }

    public void TryPlant(PlantZone zone, InventorySystem inventory)
    {
        if (zone == null || zone.IsPlanted) return;
        if (inventory == null)
        {
            return;
        }

        if (HasItem(inventory, _requiredItem))
        {
            inventory.RemoveItem(_requiredItem, 1);
            zone.Plant();
            _plantedCount++;
        }
    }

    private bool HasItem(InventorySystem inventory, ItemSO item)
    {
        foreach (var slot in inventory.slots)
        {
            if (!slot.IsEmpty && slot.item == item && slot.amount > 0)
                return true;
        }
        return false;
    }
}
