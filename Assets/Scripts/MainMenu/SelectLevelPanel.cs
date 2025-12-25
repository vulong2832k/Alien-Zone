using UnityEngine;

public class SelectLevelPanel : MonoBehaviour
{
    [SerializeField] private SelectLevelButton[] _levelButtons;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        int unlockedLevel = PlayerDataManager.Instance.CurrentData.highestUnlockedMap;

        foreach (var button in _levelButtons)
        {
            button.Setup(unlockedLevel);
        }
    }
}
