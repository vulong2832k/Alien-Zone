using UnityEngine;

public class HurtScreen : MonoBehaviour
{
    public static HurtScreen Instance { get; private set; }
    public CanvasGroup HurtFlash;

    private void Awake()
    {
        Instance = this;
    }
}
