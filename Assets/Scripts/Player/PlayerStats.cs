using UnityEngine;
using System;
public enum StatType
{
    HP,
    Strength,
    Speed,
    Mind
}
public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHPBase = 100;
    public float damageBonusPercent;
    public float moveSpeedBonusPercent;
    public float expBonusPercent;

    [Header("Allocated Points")]
    public int hpPoint;
    public int strengthPoint;
    public int speedPoint;
    public int mindPoint;

    public event Action OnStatsChanged;

    public int CurrentHP { get; private set; }
    public int MaxHP => maxHPBase + hpPoint * 5;
    public float DamageMultiplier => 1f + strengthPoint * 0.01f;
    public float MoveSpeedMultiplier => 1f + speedPoint * 0.01f;
    public float ExpMultiplier => 1f + mindPoint * 0.01f;

    private void Start()
    {
        CurrentHP = MaxHP;
    }
    public void AddPoint(StatType type)
    {
        switch (type)
        {
            case StatType.HP:
                IncreaseHP();
                break;

            case StatType.Strength:
                strengthPoint++;
                break;

            case StatType.Speed:
                speedPoint++;
                break;

            case StatType.Mind:
                mindPoint++;
                break;
        }

        OnStatsChanged?.Invoke();
    }
    private void IncreaseHP()
    {
        int oldMaxHP = MaxHP;

        hpPoint++;

        int newMaxHP = MaxHP;
        int addedHP = newMaxHP - oldMaxHP;

        CurrentHP += addedHP;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, newMaxHP);
    }
}


