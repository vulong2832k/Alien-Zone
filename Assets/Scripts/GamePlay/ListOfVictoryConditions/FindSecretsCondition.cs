using UnityEngine;

public class FindSecretsCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private int _totalSecretsCompleted = 3;
    private int _foundSecrets = 0;

    public void StartCondition()
    {
        _foundSecrets = 0;
    }

    public bool IsCompleted()
    {
        return _foundSecrets >= _totalSecretsCompleted;
    }

    public void RegisterSecretFound()
    {
        _foundSecrets++;
    }
}
