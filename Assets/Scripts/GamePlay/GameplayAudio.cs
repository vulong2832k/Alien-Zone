using UnityEngine;

public class GameplayAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(MusicType.Gameplay);
    }
}
