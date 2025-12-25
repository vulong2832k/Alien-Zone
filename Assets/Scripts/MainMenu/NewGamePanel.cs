using TMPro;
using UnityEngine;

public class NewGamePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private MainMenuController _mainMenuController;

    public void OnEnterButton()
    {
        string playerName = this.nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Player name empty!");
            return;
        }
        PlayerDataManager.Instance.CreateNewPlayer(playerName);

        _mainMenuController.ShowPanel("SelectLevelPanel");
    }
    public void OnBackButton()
    {
        _mainMenuController.ShowPanel("MainMenu");
    }
}
