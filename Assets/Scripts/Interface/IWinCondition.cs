using UnityEngine;

public interface IWinCondition
{
    bool IsCompleted();
    void StartCondition();

    string GetDescription();

}
