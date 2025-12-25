using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    private const string SAVE_KEY = "PLAYER_DATA";

    public PlayerData CurrentData { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void CreateNewPlayer(string playerName)
    {
        CurrentData = new PlayerData
        {
            playerName = playerName,
            highestUnlockedMap = 0
        };
        Save();
    }

    public void UnlockMap(int mapIndex)
    {
        if (CurrentData == null) return;
        CurrentData.highestUnlockedMap = Mathf.Max(CurrentData.highestUnlockedMap, mapIndex);
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(CurrentData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public bool Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return false;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        CurrentData = JsonUtility.FromJson<PlayerData>(json);
        return true;
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        CurrentData = null;
    }
}
