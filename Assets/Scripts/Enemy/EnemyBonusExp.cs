using UnityEngine;

public static class EnemyBonusExp
{
    public static int GetBaseExp(TypeEnemy typeEnemy)
    {
        return typeEnemy switch
        {
            TypeEnemy.ranger => 30,
            TypeEnemy.Explosion => 80,
            TypeEnemy.boss => 500,
            _ => 15
        };
    }
    public static float GetMultiplier(LevelEnemy levelEnemy)
    {
        return levelEnemy switch
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
    public static int GetExpReward(TypeEnemy typeEnemy, LevelEnemy levelEnemy)
    {
        int baseExp = GetBaseExp(typeEnemy);
        float multiplier = GetMultiplier(levelEnemy);
        return Mathf.RoundToInt(baseExp * multiplier);
    }
}
