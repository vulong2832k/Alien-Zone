using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerStatsUI;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private PlayerLevelSystem _levelSystem;

    [Header("UI:")]
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private Button btnApply;

    [System.Serializable]
    public class StatRow
    {
        public StatType type;
        public TextMeshProUGUI txtValue;
        public Button btnPlus;
        public Button btnMinus;
    }

    [SerializeField] private List<StatRow> _statRows;

    private Dictionary<StatType, int> _pendingPoints = new();
    private int _pendingUsedPoints;

    private void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        while (PlayerController.Instance == null)
            yield return null;

        var player = PlayerController.Instance;

        _stats = player.GetComponent<PlayerStats>();
        _levelSystem = player.GetComponent<PlayerLevelSystem>();

        if (_stats == null || _levelSystem == null)
        {
            yield break;
        }

        Init();
        RefreshUI();

        _levelSystem.OnAvailableStatPointChanged += _ => RefreshUI();
    }

    private void Init()
    {
        foreach (var row in _statRows)
        {
            _pendingPoints[row.type] = 0;

            row.btnPlus.onClick.AddListener(() => AddPoint(row.type));
            row.btnMinus.onClick.AddListener(() => RemovePoint(row.type));
        }

        btnApply.onClick.AddListener(ApplyStats);
    }
    private void AddPoint(StatType type)
    {
        if (_pendingUsedPoints >= _levelSystem.availableStatPoints)
            return;

        _pendingPoints[type]++;
        _pendingUsedPoints++;
        RefreshUI();
    }

    private void RemovePoint(StatType type)
    {
        if (_pendingPoints[type] <= 0)
            return;

        _pendingPoints[type]--;
        _pendingUsedPoints--;
        RefreshUI();
    }
    private void ApplyStats()
    {
        if (_pendingUsedPoints <= 0)
            return;

        foreach (var kvp in _pendingPoints.ToList())
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                _stats.AddPoint(kvp.Key);
                _levelSystem.UseStatPoint();
            }
        }

        foreach (var key in _pendingPoints.Keys.ToList())
            _pendingPoints[key] = 0;

        _pendingUsedPoints = 0;

        RefreshUI();
    }


    private void RefreshUI()
    {
        _txtAvailablePoints.text =
            $"Chỉ số có thể cộng: {_levelSystem.availableStatPoints - _pendingUsedPoints}";

        foreach (var row in _statRows)
        {
            int baseValue = GetBaseValue(row.type);
            int pending = _pendingPoints[row.type];

            int current = baseValue + pending;
            int max = GetBaseValue(row.type);

            row.txtValue.text = $"{current:000}/{max:000}";

            row.btnMinus.interactable = pending > 0;
            row.btnPlus.interactable =
                _pendingUsedPoints < _levelSystem.availableStatPoints;
        }

        btnApply.interactable = _pendingUsedPoints > 0;
    }

    private int GetBaseValue(StatType type)
    {
        return type switch
        {
            StatType.HP => _stats.hpPoint,
            StatType.Strength => _stats.strengthPoint,
            StatType.Speed => _stats.speedPoint,
            StatType.Mind => _stats.mindPoint,
            _ => 0
        };
    }
}
