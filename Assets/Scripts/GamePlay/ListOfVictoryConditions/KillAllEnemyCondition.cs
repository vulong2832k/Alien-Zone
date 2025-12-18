using UnityEngine;

public class KillAllEnemiesCondition : MonoBehaviour, IWinCondition
{
    private SpawnZone[] _zones;

    public void StartCondition()
    {
        _zones = Object.FindObjectsByType<SpawnZone>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
    }

    public bool IsCompleted()
    {
        if (_zones == null || _zones.Length == 0) return false;

        foreach (var zone in _zones)
        {
            if (!zone.IsCleared) return false;
        }
        return true;
    }

    public string GetDescription()
    {
        if (_zones == null || _zones.Length == 0)
            return "Kill All Enemies";

        int total = 0;
        int killed = 0;

        foreach (var z in _zones)
        {
            total += z.CalculateTotalEnemies();
            killed += z.KilledEnemies;
        }

        return $"Kill All Enemies: {killed} / {total}";
    }

}
