using UnityEngine;

[CreateAssetMenu(fileName = "HeadArmorSO", menuName = "Inventory/HeadArmorSO")]
public class HeadArmorSO : ScriptableObject
{
    [Header("Stats: ")]
    public int bonusHP;
    public int bonusDamageWeapon;
}
