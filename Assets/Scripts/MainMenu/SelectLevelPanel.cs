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
        var manager = PlayerDataManager.Instance;

        if (manager == null || manager.CurrentData == null)
            return;

        int unlockedLevel = manager.CurrentData.highestUnlockedMap;

        foreach (var button in _levelButtons)
        {
            button.Setup(unlockedLevel);
        }
    }

    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
