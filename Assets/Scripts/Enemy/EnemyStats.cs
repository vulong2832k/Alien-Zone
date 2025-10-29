using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int HP { get; private set; }
    public int Damage { get; private set; }
    public LevelEnemy Level { get; private set; }
    public int ExpReward { get; private set; }

    public void Init(LevelEnemy level)
    {
        Level = level;

        float multiplier = EnemyLevelHelper.GetMultiplier(level);

        EnemySO baseData = GetComponent<EnemyController>().dataEnemy;
        HP = Mathf.RoundToInt(baseData.MaxHP * multiplier);
        Damage = Mathf.RoundToInt(baseData.Damage * multiplier);
        ExpReward = EnemyBonusExp.GetExpReward(baseData.TypeEnemy, level);

        GetComponent<EnemyController>()?.ApplyStats();
    }
}
