using UnityEngine;

public enum MedicineType
{
    low,
    normal,
    high,
}

[CreateAssetMenu(fileName = "MedicineSO", menuName = "Inventory/MedicineSO")]
public class MedicineSO : ItemSO
{
    public int recoveryHP;

    public MedicineType medicineType;
}
