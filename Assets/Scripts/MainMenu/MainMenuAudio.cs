using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(MusicType.MainMenu);
        }
    }
}
