using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance { get; private set; }
    public CanvasGroup DeathFlash;

    private void Awake()
    {
        Instance = this;
    }
}
