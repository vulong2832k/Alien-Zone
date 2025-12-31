using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _victoryPanel;

    [Header("Buttons:")]
    [SerializeField] private Button _completeBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _exitBtn;

    [Header("Progress Text: ")]
    [SerializeField] private TextMeshProUGUI _totalTimeText;
    [SerializeField] private TextMeshProUGUI _totalKillText;
    [SerializeField] private TextMeshProUGUI _totalItemLootText;
    [SerializeField] private TextMeshProUGUI _totalDamage;
    [SerializeField] private TextMeshProUGUI _totalDamageTaken;
    [SerializeField] private TextMeshProUGUI _totalChestLoot;

    private void Awake()
    {
        _completeBtn.onClick.AddListener(OnNextLevelClicked);
        _settingBtn.onClick.AddListener(OnSettingClicked);
        _exitBtn.onClick.AddListener(OnExitClicked);
    }
    private void OnDestroy()
    {
        _completeBtn.onClick.RemoveListener(OnNextLevelClicked);
        _settingBtn.onClick.RemoveListener(OnSettingClicked);
        _exitBtn.onClick.RemoveListener(OnExitClicked);
    }
    private void OnNextLevelClicked()
    {
        Time.timeScale = 1f;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            LoadingManager.Instance.LoadScene("MainMenu");
        }
    }

    private void OnSettingClicked()
    {
        UIPopupManager.Instance.ShowSettingPanel();
    }
    private void OnExitClicked()
    {
        Time.timeScale = 1f;
        LoadingManager.Instance.LoadScene("MainMenu");
    }

    public void SetData(GameResultData data)
    {
        _totalTimeText.text = $"Total Time: {FormatTime(data.totalTime)}";
        _totalKillText.text = $"Kill Enemy: {data.totalKill.ToString()}";
        _totalItemLootText.text = $"Total Items Picked:  {data.totalItemLoot.ToString()}";
        _totalDamage.text = $"Total Damage: {data.totalDamage.ToString()}";
        _totalDamageTaken.text = $"Total Damage Taken: {data.totalDamageTaken.ToString()}";
        _totalChestLoot.text = $"Total Chest Picked: {data.totalChestLoot.ToString()}";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
