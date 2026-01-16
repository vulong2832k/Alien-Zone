using UnityEngine;
public enum ItemType
{
    Weapon,
    HeadArmor,
    Armor,
    Medicine,
    Grenade,
    Ammo,
    QuestC4
}
public enum EquipmentSlot
{
    None,
    Head,
    Body,
    Weapon
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Inventory/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("General Info")]
    public string itemId;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject worldPrefab;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStack;

    [Header("References")]
    public GunAttributes gunAttributes;

    [Header("Equipment")]
    public EquipmentSlot equipmentSlot;

    private void OnValidate()
    {
        switch (itemType)
        {
            case ItemType.Weapon:
                isStackable = false;
                maxStack = 1;
                equipmentSlot = EquipmentSlot.Weapon;
                break;

            case ItemType.HeadArmor:
                isStackable = false;
                maxStack = 1;
                equipmentSlot = EquipmentSlot.Head;
                break;

            case ItemType.Armor:
                isStackable = false;
                maxStack = 1;
                equipmentSlot = EquipmentSlot.Body;
                break;

            case ItemType.Medicine:
            case ItemType.Grenade:
            case ItemType.QuestC4:
                isStackable = true;
                maxStack = 5;
                equipmentSlot = EquipmentSlot.None;
                break;

            case ItemType.Ammo:
                isStackable = true;
                maxStack = 20;
                equipmentSlot = EquipmentSlot.None;
                break;
        }
    }

}
