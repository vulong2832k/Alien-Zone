using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorInstance
{
    public ArmorSO armorSO;

    public int bonusMaxHP;
    public float bonusMoveSpeed;
    public int bonusSlotItem;

    private Dictionary<GunType, int> ammoBonusMap;

    public ArmorInstance(ArmorSO so)
    {
        armorSO = so;

        bonusMaxHP = Random.Range(
            so.bonusMaxHPRange.x,
            so.bonusMaxHPRange.y + 1
        );

        bonusMoveSpeed = Random.Range(
            so.bonusMoveSpeedRange.x,
            so.bonusMoveSpeedRange.y
        );

        bonusSlotItem = so.baseSlotItem;

        ammoBonusMap = new Dictionary<GunType, int>();

        foreach (var bonus in so.ammoBonuses)
        {
            ammoBonusMap.Add(
                bonus.gunType,
                bonus.bonusAmmo
            );
        }
    }

    public int GetBonusAmmo(GunType gunType)
    {
        if (ammoBonusMap.TryGetValue(gunType, out int value))
            return value;

        if (ammoBonusMap.TryGetValue(GunType.Universal, out value))
            return value;

        return 0;
    }
}
