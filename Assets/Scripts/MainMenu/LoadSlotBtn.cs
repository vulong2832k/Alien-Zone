using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlotBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _loadSlotBtn;

    private string _slotId;

    public void Setup(string slotId, PlayerData data)
    {
        _slotId = slotId;

        _loadSlotBtn.onClick.RemoveAllListeners();

        _nameText.text = data.playerName;
        _loadSlotBtn.interactable = true;

        _loadSlotBtn.onClick.AddListener(OnLoad);
    }
    public void SetupEmpty(string slotId)
    {
        _slotId = slotId;

        _nameText.text = "Empty Slot";
        _loadSlotBtn.interactable = true;

        _loadSlotBtn.onClick.RemoveAllListeners();
        _loadSlotBtn.onClick.AddListener(OnCreateNew);
    }
    private void OnLoad()
    {
        PlayerDataManager.Instance.SetCurrentSlot(_slotId);

        int levelIndex = PlayerDataManager.Instance.CurrentData.currentLevelIndex;
        LoadingManager.Instance.LoadScene($"Level_{levelIndex + 1}");
    }
    private void OnCreateNew()
    {
        MainMenuController.Instance.ShowPanel("NewGamePanel");
    }
}
