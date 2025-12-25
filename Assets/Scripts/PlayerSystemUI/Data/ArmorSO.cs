using UnityEngine;

[CreateAssetMenu(fileName = "ArmorSO", menuName = "Inventory/ArmorSO")]
public class ArmorSO : ItemSO
{
    [Header("Stats: ")]
    public int baseMaxHP;
    public int baseCurrentAmmo;
    public float baseMoveSpeed;
    public int baseSlotItem;

    [Header("Random Range")]
    public Vector2Int bonusMaxHPRange;
    public Vector2Int bonusCurrentAmmoRange;
    public Vector2 bonusMoveSpeedRange;
}
