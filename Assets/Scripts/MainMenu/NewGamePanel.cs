using TMPro;
using UnityEngine;

public class NewGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private MainMenuController _mainMenuController;
    public void OnEnterButton()
    {
        Debug.Log("ENTER CLICKED");

        string playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Name empty");
            return;
        }

        string slotId = PlayerDataManager.Instance.GetFirstEmptySlot();
        if (string.IsNullOrEmpty(slotId))
        {
            Debug.Log("All slots are full");
            return;
        }

        PlayerDataManager.Instance.CreateNewPlayer(slotId, playerName);
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
