using UnityEngine;
using System;

public class PlayerLevelSystem : MonoBehaviour
{
    public event Action<int> OnAvailableStatPointChanged;

    public int level = 1;
    public int currentXP;
    public int xpToNextLevel = 100;
    public float xpGrowthRate = 1.2f;

    public int availableStatPoints;

    public event Action<int, int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    public void AddXP(int amount, float expMultiplier)
    {
        int finalXP = Mathf.RoundToInt(amount * expMultiplier);
        currentXP += finalXP;

        if (currentXP >= xpToNextLevel)
            LevelUp();

        OnXPChanged?.Invoke(currentXP, xpToNextLevel, level);
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);

        availableStatPoints += 3;

        OnAvailableStatPointChanged?.Invoke(availableStatPoints);

        OnLevelUp?.Invoke(level);
    }

    public bool UseStatPoint()
    {
        if (availableStatPoints <= 0) return false;
        availableStatPoints--;
        return true;
    }
}
