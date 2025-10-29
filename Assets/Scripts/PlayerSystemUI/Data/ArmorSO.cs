using UnityEngine;

[CreateAssetMenu(fileName = "ArmorSO", menuName = "Inventory/ArmorSO")]
public class ArmorSO : ItemSO
{
    [Header("Stats: ")]
    public int bonusMaxHP;
    public int bonusCurrentAmmo;
    public float bonusMoveSpeed;
    public int bonusSlotItem;
}
