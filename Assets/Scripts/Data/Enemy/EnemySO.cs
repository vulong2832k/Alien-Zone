using UnityEngine;

public enum TypeEnemy
{
    normal = 1,
    ranger = 2,
    boss = 3,
    Explosion = 4,
}
public enum LevelEnemy
{
    Default = 0,
    Green = 1,
    Blue = 2,
    Violet = 3,
    Yellow = 4,
    Orange = 5,
    Red = 6
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "EnemySO/DataEnemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _damage;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackRange;
    [SerializeField] private TypeEnemy _typeEnemy;
    [SerializeField] private LevelEnemy _levelEnemy = LevelEnemy.Default;
    [SerializeField] private EnemyAttackSO _attackStrategy;


    public string Name => _name;
    public int MaxHP => _maxHP;
    public int Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float AttackCooldown => _attackCooldown;
    public float AttackRange => _attackRange;
    public TypeEnemy TypeEnemy => _typeEnemy;
    public LevelEnemy LevelEnemy => _levelEnemy;
    public EnemyAttackSO AttackStrategy => _attackStrategy;
}
