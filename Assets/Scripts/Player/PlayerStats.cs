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
    [SerializeField] private float baseHPRecovery = 0f;

    [Header("Allocated Points")]
    public int hpPoint;
    public int strengthPoint;
    public int speedPoint;
    public int mindPoint;

    // ================= BONUS FROM EQUIPMENT =================
    private int bonusMaxHP;
    private float bonusMoveSpeedPercent;
    private float bonusHPRecovery;

    // ================= RUNTIME =================
    public int CurrentHP { get; private set; }

    public event Action OnStatsChanged;

    // ================= CALCULATED =================
    public int MaxHP => baseMaxHP + bonusMaxHP + hpPoint * 5;

    public float HPRecovery => baseHPRecovery + bonusHPRecovery;

    public float DamageMultiplier => 1f + strengthPoint * 0.01f;

    public float MoveSpeedMultiplier =>
        1f + speedPoint * 0.01f + bonusMoveSpeedPercent;

    public float ExpMultiplier => 1f + mindPoint * 0.01f;

    private void Start()
    {
        CurrentHP = MaxHP;
    }

    private void OnEnable()
    {
        if (EquipmentSystem.Instance != null)
            EquipmentSystem.Instance.OnEquipmentChanged += RecalculateStats;
    }

    private void OnDisable()
    {
        if (EquipmentSystem.Instance != null)
            EquipmentSystem.Instance.OnEquipmentChanged -= RecalculateStats;
    }
    private void Update()
    {
        if (HPRecovery <= 0f) return;
        if (CurrentHP >= MaxHP) return;

        CurrentHP += Mathf.CeilToInt(HPRecovery * Time.deltaTime);
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
    }
    // ================= POINT =================
    public void AddPoint(StatType type)
    {
        int oldMaxHP = MaxHP;

        switch (type)
        {
            case StatType.HP:
                hpPoint++;
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

        if (type == StatType.HP)
        {
            int delta = MaxHP - oldMaxHP;
            CurrentHP += delta;
        }

        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        OnStatsChanged?.Invoke();
    }

    // ================= EQUIPMENT =================
    public void RecalculateStats()
    {
        bonusMaxHP = 0;
        bonusMoveSpeedPercent = 0;
        bonusHPRecovery = 0f;

        var equip = EquipmentSystem.Instance;

        if (equip.bodyArmor != null)
        {
            bonusMaxHP += equip.bodyArmor.bonusMaxHP;
            bonusMoveSpeedPercent += equip.bodyArmor.bonusMoveSpeedPercent;
        }

        if (equip.headArmor != null)
        {
            bonusMaxHP += equip.headArmor.bonusMaxHP;
            bonusHPRecovery += equip.headArmor.baseHPRecovery;
        }

        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
        OnStatsChanged?.Invoke();
    }
}
