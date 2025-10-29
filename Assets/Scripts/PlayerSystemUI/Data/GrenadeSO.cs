using UnityEngine;

public enum GrenadeType
{
    low,
    normal,
    high,
}

[CreateAssetMenu(fileName = "GrenadeSO", menuName = "Inventory/GrenadeSO")]

public class GrenadeSO : ItemSO
{
    public int damage;
    public float radius;

    public GrenadeType grenadeType;
}
