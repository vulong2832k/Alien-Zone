using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private Button _selectBtn;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private TextMeshProUGUI _levelText;

    public void Setup(int unlockLevel)
    {
        _levelText.text = $"Level {_levelIndex + 1}";

        bool unlocked = _levelIndex <= unlockLevel;

        _selectBtn.interactable = unlocked;
        _lockIcon.SetActive(!unlocked);
    }
    public void OnClick()
    {
        LevelSelectManager.Instance.SelectLevel(_levelIndex);
    }
}
