using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankPanel : MonoBehaviour
{
    [SerializeField] private Transform _listRankRoot;
    [SerializeField] private RankPlayerUI _rankPrefab;

    private const int MAX_SLOT = 5;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        Clear();

        List<RankEntry> ranks = LoadRankData();

        var sorted = ranks
            .OrderByDescending(r => r.highLevel)
            .ThenByDescending(r => r.totalKill)
            .ThenBy(r => r.playerTime)
            .ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            RankPlayerUI item = Instantiate(_rankPrefab, _listRankRoot);
            item.Setup(i + 1, sorted[i]);
        }
    }

    private List<RankEntry> LoadRankData()
    {
        List<RankEntry> list = new();

        for (int i = 0; i < MAX_SLOT; i++)
        {
            string slotId = $"slot_{i}";
            PlayerData data = PlayerDataManager.Instance.LoadSlot(slotId);

            if (data == null) continue;

            list.Add(new RankEntry
            {
                playerName = data.playerName,
                highLevel = data.highestUnlockedMap,
                totalKill = data.totalKill,
                playerTime = data.playTime
            });
        }

        return list;
    }

    private void Clear()
    {
        foreach (Transform child in _listRankRoot)
            Destroy(child.gameObject);
    }
    public void ExitPanel()
    {
        if (MainMenuController.Instance != null)
        {
            MainMenuController.Instance.ExitCurrentPanel();
        }
    }
}
