using UnityEngine;

[CreateAssetMenu(fileName = "AmmoSO", menuName = "Inventory/AmmoSO")]
public class AmmoSO : ItemSO
{
    [Range(0f, 1f)] public float minPercentRecover = 0.05f;
    [Range(0f, 1f)] public float maxPercentRecover = 0.15f;
}
