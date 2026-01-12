using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmorInstance : ItemInstance
{
    public int totalMaxHP;
    public float totalMoveSpeed;
    public int bonusSlotItem;

    private Dictionary<GunType, int> ammoBonusMap;

    public override bool IsStackable => false;

    public ArmorInstance(ArmorSO so)
    {
        itemSO = so;
        amount = 1;

        int bonusHP = Random.Range(so.bonusMaxHPRange.x, so.bonusMaxHPRange.y + 1);

        float bonusSpeed = Random.Range(so.bonusMoveSpeedRange.x, so.bonusMoveSpeedRange.y);

        totalMaxHP = so.baseMaxHP + bonusHP;
        totalMoveSpeed = so.baseMoveSpeed + bonusSpeed;
        bonusSlotItem = so.baseSlotItem;

        ammoBonusMap = new Dictionary<GunType, int>();
        foreach (var bonus in so.ammoBonuses)
        {
            ammoBonusMap[bonus.gunType] = bonus.bonusAmmo;
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
