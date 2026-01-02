using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButton : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _levelIndex;

    [Header("UI")]
    [SerializeField] private Button _selectBtn;
    [SerializeField] private Image _stateImage;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Sprite _unlockSprite;
    [SerializeField] private TextMeshProUGUI _levelText;

    public void Setup(int unlockLevel)
    {
        _levelText.text = $"Level {_levelIndex + 1}";

        bool unlocked = _levelIndex <= unlockLevel;

        _selectBtn.interactable = unlocked;
        _stateImage.sprite = unlocked ? _unlockSprite : _lockSprite;
    }

    public void OnClick()
    {
        if (!_selectBtn.interactable)
            return;

        LevelSelectManager.Instance.SelectLevel(_levelIndex);
    }
}
