using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorSO", menuName = "Inventory/ArmorSO")]
public class ArmorSO : ItemSO
{
    [System.Serializable]
    public class GunAmmoBonus
    {
        public GunType gunType;
        public int bonusAmmo;
    }

    [Header("Stats: ")]
    public int baseMaxHP;
    public float baseMoveSpeed;
    public int baseSlotItem;

    [Header("Ammo Bonus By Gun Type")]
    public List<GunAmmoBonus> ammoBonuses;

    public int GetBonusAmmo(GunType gunType)
    {
        foreach (var bonus in ammoBonuses)
        {
            if (bonus.gunType == gunType || bonus.gunType == GunType.Universal)
                return bonus.bonusAmmo;
        }
        return 0;
    }
    [Header("Random Range")]
    public Vector2Int bonusMaxHPRange;
    public Vector2 bonusMoveSpeedRange;
}
