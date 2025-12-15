using TMPro;
using UnityEngine;

public class LootChatItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private void Reset()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void SetText(string msg)
    {
        _text.text = msg;
    }
}
