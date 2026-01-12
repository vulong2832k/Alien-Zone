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
    public static PlayerStats Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Base Stats")]
    [SerializeField] private int baseMaxHP = 100;

    [Header("Bonus Stats (Equipment / Buff)")]
    private int bonusMaxHP;
    private float bonusMoveSpeedPercent;

    [Header("Allocated Points")]
    public int hpPoint;
    public int strengthPoint;
    public int speedPoint;
    public int mindPoint;

    public event Action OnStatsChanged;

    public int CurrentHP { get; private set; }

    public int MaxHP => baseMaxHP + bonusMaxHP + hpPoint * 5;

    public float DamageMultiplier => 1f + strengthPoint * 0.01f;

    public float MoveSpeedMultiplier => 1f + speedPoint * 0.01f + bonusMoveSpeedPercent;

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
        int oldMax = MaxHP;
        hpPoint++;
        int delta = MaxHP - oldMax;

        CurrentHP += delta;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
    }

    public void RecalculateStats(ArmorInstance body, ArmorInstance head)
    {
        bonusMaxHP = 0;
        bonusMoveSpeedPercent = 0;

        if (body != null)
        {
            bonusMaxHP += body.totalMaxHP;
            bonusMoveSpeedPercent += body.totalMoveSpeed;
        }

        if (head != null)
        {
            
        }

        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        OnStatsChanged?.Invoke();
    }
}



