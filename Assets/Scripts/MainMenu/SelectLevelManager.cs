using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public static LevelSelectManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SelectLevel(int levelIndex)
    {
        Debug.Log("SelectLevel called");

        if (PlayerDataManager.Instance == null)
            Debug.LogError("PlayerDataManager.Instance = NULL");

        if (LoadingManager.Instance == null)
            Debug.LogError("LoadingManager.Instance = NULL");

        PlayerDataManager.Instance.SetCurrentLevel(levelIndex);
        LoadingManager.Instance.LoadScene($"Level_{levelIndex + 1}");
    }

}
