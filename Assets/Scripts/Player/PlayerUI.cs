using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    [Header("References: ")]
    [SerializeField] private PlayerController _player;

    [Header("Health: ")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthFill;

    [Header("XP: ")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _xpFill;

    private void Start()
    {
        InitComponent();

        _player.OnHealthChanged += UpdateHealthUI;
        _player.OnXPChanged += UpdateXPUI;

        UpdateHealthUI(_player.CurrentHP, _player.MaxHP);
        UpdateXPUI(0, 100, 1);
    }
    private void InitComponent()
    {
        _player  = FindAnyObjectByType<PlayerController>();

        _healthText = GameObject.Find("CountHealth").GetComponent<TextMeshProUGUI>();
        _healthFill = GameObject.Find("CurrentHealthImg").GetComponent<Image>();

        _levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        _xpFill = GameObject.Find("FillLevelCurrent").GetComponent<Image>();
    }
    private void OnDestroy()
    {
        _player.OnHealthChanged -= UpdateHealthUI;
        _player.OnXPChanged -= UpdateXPUI;
    }
    private void UpdateHealthUI(int current, int max)
    {
        float targetFill = (float)current / max;
        _healthFill.DOFillAmount(targetFill, 0.25f).SetEase(Ease.OutSine);
        _healthText.text = $"{current} / {max}";
    }

    private void UpdateXPUI(int xp, int xpToNext, int level)
    {
        _levelText.text = $"Level: {level}";
        float targetFill = (float)xp / xpToNext;
        _xpFill.DOFillAmount(targetFill, 0.3f).SetEase(Ease.OutSine);
    }
}
