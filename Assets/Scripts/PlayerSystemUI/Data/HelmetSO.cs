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

    [Header("Stats: ")]
    public int baseHP;
    public float baseHPRecovery;

    [Header("Random Range")]
    public Vector2Int bonusMaxHPRange;
    public Vector2 bonusHPRecoveryRange;
}
