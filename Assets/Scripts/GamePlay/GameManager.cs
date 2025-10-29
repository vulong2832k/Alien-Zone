using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IWinCondition[] _winConditions;

    public bool IsGameOver {  get; private set; }
    public bool IsVictory { get; private set; }

    private void Start()
    {
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
            foreach (var condition in _winConditions)
            {
                if (condition.IsCompleted())
                {
                    VictoryGame();
                    Debug.Log("YOU WIN!");
                    break;
                }
            }
        }

        ResetGame();
    }

    private void ResetGame()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
    private void VictoryGame()
    {
        IsGameOver = true;
        IsVictory = true;
    }
    public void LoseGame()
    {
        IsGameOver = true;
        IsVictory = false;
    }
}
