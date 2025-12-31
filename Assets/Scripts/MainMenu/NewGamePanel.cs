using TMPro;
using UnityEngine;

public class NewGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private MainMenuController _mainMenuController;

    private string _slotId;

    public void SetSlot(string slotId)
    {
        _slotId = slotId;
    }

    public void OnEnterButton()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            return;
        }

        PlayerDataManager.Instance.CreateNewPlayer(_slotId, playerName);

        _mainMenuController.ShowPanel("SelectLevelPanel");
    }

    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
