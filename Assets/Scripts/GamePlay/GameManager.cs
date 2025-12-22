using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerController _player;

    [SerializeField] private IWinCondition[] _winConditions;
    [SerializeField] private ExitZone _exitZone;

    public bool IsGameOver {  get; private set; }
    public bool IsVictory { get; private set; }

    [Header("Data:")]
    public GameResultData ResultData { get; private set; }

    [SerializeField] private float _playTime;
    [SerializeField] private int _totalKill;
    [SerializeField] private int _totalItemLoot;
    [SerializeField] private int _totalDamage;
    [SerializeField] private int _totalDamageTaken;
    [SerializeField] private int _totalChestLoot;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        
    }
    private void Start()
    {
        _player = PlayerController.Instance;

        if (_player != null)
            _player.OnPlayerDead += HandlePlayerDead;

        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.LockCursor();
        }
        else
        {
            Debug.LogError("Gán cursor vô!");
        }

        _winConditions = GetComponentsInChildren<IWinCondition>();

        foreach (var condition in _winConditions)
        {
            condition.StartCondition();
        }
    }
    private void Update()
    {
        if (!IsGameOver)
        {
            _playTime += Time.deltaTime;

            foreach (var condition in _winConditions)
            {
                if (condition.IsCompleted())
                {
                    VictoryGame();
                    _exitZone.ActivateExitZone();
                    break;
                }
            }
        }

        ResetGame();
    }
    private void OnDestroy()
    {
        if (_player != null)
            _player.OnPlayerDead -= HandlePlayerDead;
    }
    private void HandlePlayerDead()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        IsVictory = false;

        StartCoroutine(DelayLoseUI());
    }

    private IEnumerator DelayLoseUI()
    {
        yield return new WaitForSecondsRealtime(3f);

        UIPopupManager.Instance.ShowLosePanel();
    }

    private void ResetGame()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = 1f;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
    public void LoseGame()
    {
        IsGameOver = true;
        IsVictory = false;
    }
    private void VictoryGame()
    {
        IsGameOver = true;
        IsVictory = true;

        ResultData = new GameResultData
        {
            totalTime = _playTime,
            totalKill = _totalKill,
            totalItemLoot = _totalItemLoot,
            totalDamage = _totalDamage,
            totalDamageTaken = _totalDamageTaken,
            totalChestLoot = _totalChestLoot
        };

        UIPopupManager.Instance.ShowWinPanel(ResultData);
    }
    public void AddKill(int amount  = 1)
    {
        _totalKill += amount;
    }
    public void AddDamage(int amount)
    {
        _totalDamage += amount;
    }
    public void AddDamageTaken(int amount)
    {
        _totalDamageTaken += amount;
    }
    public void AddChestLoot(int amount = 1)
    {
        _totalChestLoot += amount;
    }
    public void AddItemLoot(int amount = 1)
    {
        _totalItemLoot += amount;
    }
}
