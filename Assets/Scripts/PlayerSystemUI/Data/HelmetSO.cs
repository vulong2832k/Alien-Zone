using UnityEngine;

[CreateAssetMenu(fileName = "HelmetSO", menuName = "Inventory/HelmetSO")]
public class HelmetSO : ItemSO
{
    [System.Serializable]
    public class GunDataManager
    {
        public GunType gunType;
        public int bonusDamage;
    }

    [Header("Stats Bonus")]
    public int bonusMaxHP;
    public float bonusMoveSpeedPercent;

    [Header("Stats: ")]
    public int baseHP;
    public float baseHPRecovery;
}
