using UnityEngine;

[System.Serializable]
public class ArmorInstance
{
    public ArmorSO armorSO;

    public int bonusMaxHP;
    public int bonusCurrentAmmo;
    public float bonusMoveSpeed;
    public int bonusSlotItem;

    public ArmorInstance(ArmorSO so)
    {
        armorSO = so;

        bonusMaxHP = Random.Range(
            so.bonusMaxHPRange.x,
            so.bonusMaxHPRange.y + 1
        );

        bonusCurrentAmmo = Random.Range(
            so.bonusCurrentAmmoRange.x,
            so.bonusCurrentAmmoRange.y + 1
        );

        bonusMoveSpeed = Random.Range(
            so.bonusMoveSpeedRange.x,
            so.bonusMoveSpeedRange.y
        );

        bonusSlotItem = so.baseSlotItem;
    }
}
