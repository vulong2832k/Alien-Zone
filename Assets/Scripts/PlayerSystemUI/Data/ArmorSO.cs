using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/ArmorSO")]
public class ArmorSO : ItemSO
{
    [Header("Stats Bonus")]
    public int bonusMaxHP;
    public float bonusMoveSpeedPercent;
}
