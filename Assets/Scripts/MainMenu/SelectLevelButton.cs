using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButton : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int _levelIndex;

    [Header("UI")]
    [SerializeField] private Button _selectBtn;
    [SerializeField] private GameObject _unlockImage;
    [SerializeField] private GameObject _lockImage;
    [SerializeField] private TextMeshProUGUI _levelText;

    public void Setup(int unlockLevel)
    {
        _levelText.text = $"Level {_levelIndex + 1}";

        bool unlocked = _levelIndex <= unlockLevel;

        _selectBtn.interactable = unlocked;

        _unlockImage.SetActive(unlocked);
        _lockImage.SetActive(!unlocked);
    }

    public void OnClick()
    {
        if (!_selectBtn.interactable)
            return;

        LevelSelectManager.Instance.SelectLevel(_levelIndex);
    }
}
