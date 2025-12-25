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
        //PlayerDataManager.Instance.SetCurrentLevel(levelIndex);

        //LoadingManager.Instance.LoadScene($"Map{levelIndex + 1}");
    }
}
