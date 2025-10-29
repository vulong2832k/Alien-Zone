using UnityEngine;

public static class EnemyLevelHelper
{
    public static float GetMultiplier(LevelEnemy level)
    {
        return level switch
        {
            LevelEnemy.Green => 1.2f,
            LevelEnemy.Blue => 2f,
            LevelEnemy.Violet => 3f,
            LevelEnemy.Yellow => 5f,
            LevelEnemy.Orange => 7f,
            LevelEnemy.Red => 10f,
            _ => 1f
        };
    }

    public static Color GetColor(LevelEnemy level)
    {
        return level switch
        {
            LevelEnemy.Green => Color.green,
            LevelEnemy.Blue => Color.blue,
            LevelEnemy.Violet => new Color(0.5f, 0f, 1f),
            LevelEnemy.Yellow => Color.yellow,
            LevelEnemy.Orange => new Color(1f, 0.5f, 0f),
            LevelEnemy.Red => Color.red,
            _ => Color.white
        };
    }
}
