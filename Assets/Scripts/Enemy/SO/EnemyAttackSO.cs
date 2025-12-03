using UnityEngine;

public class AttackResult
{
    public int damage;
    public Transform attacker;
    public Transform target;
    public string extraString;
}

public abstract class EnemyAttackSO : ScriptableObject
{
    public float AttackDuration = 1f;
    public abstract AttackResult EnemyAttack(Transform enemy, Transform target, int damage);
}
