using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(MusicType.MainMenu);
    }
}
