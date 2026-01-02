using UnityEngine;

public class VictoryPanelAudio : MonoBehaviour
{
    private void OnEnable()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.PlayMusic(MusicType.Victory);
    }
}
