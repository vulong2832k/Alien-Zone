using UnityEngine;

public class SurviveTimerCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private float _timeCompleted;
    private float _timer = 0;
    public void StartCondition()
    {
        _timer = 0f;
    }
    public bool IsCompleted()
    {
        _timer += Time.deltaTime;
        return _timer >= _timeCompleted;
    }

}
