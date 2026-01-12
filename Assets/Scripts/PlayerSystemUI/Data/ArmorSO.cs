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

    [Header("Base Stats")]
    public int baseMaxHP;
    public float baseMoveSpeed;
    public int baseSlotItem;

    [Header("Random Bonus Range")]
    public Vector2Int bonusMaxHPRange;
    public Vector2 bonusMoveSpeedRange;

    [Header("Ammo Bonus By Gun Type")]
    public List<GunAmmoBonus> ammoBonuses;
}
