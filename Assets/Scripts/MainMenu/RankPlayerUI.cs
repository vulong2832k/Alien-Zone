using TMPro;
using UnityEngine;

public class RankPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtRank;
    [SerializeField] private TextMeshProUGUI _txtName;
    [SerializeField] private TextMeshProUGUI _txtTime;
    [SerializeField] private TextMeshProUGUI _txtHighLevel;
    [SerializeField] private TextMeshProUGUI _txtTotalKill;

    public void Setup(int rank, RankEntry data)
    {
        _txtRank.text = rank.ToString();
        _txtName.text = data.playerName;
        _txtHighLevel.text = data.highLevel.ToString();
        _txtTotalKill.text = data.totalKill.ToString();
        _txtTime.text = FormatTime(data.playerTime);
    }

    private string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }
}
