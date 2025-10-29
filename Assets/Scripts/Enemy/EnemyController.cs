using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Attributes: ")]
    [SerializeField] private int _currentHP;
    private float _cooldown;

    [Header("References: ")]
    public EnemySO dataEnemy;
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private string _enemyKey = "Enemy";
    [SerializeField] private EnemyStats _stats;

    [Header("Components: ")]
    private NavMeshAgent _agent;

    [Header("Logic: ")]
    private bool _isAttacking = false;
    private bool _playerInRange = false;
    public bool IsAlive { get; private set; } = true;

    //Event
    public event Action<EnemyController> OnEnemyDie;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _stats = GetComponent<EnemyStats>();
        _targetPlayer = FindAnyObjectByType<PlayerController>().transform;
    }
    private void OnEnable()
    {
        _cooldown = 0f;
        _isAttacking = false;
        _playerInRange = false;

        if (_agent != null)
            _agent.isStopped = false;
    }
    private void Start()
    {
        StartCoroutine(WaitForPlayer());

        if (dataEnemy != null)
            _agent.speed = dataEnemy.MoveSpeed;
    }
    private void Update()
    {
        CheckTypeAttack();
    }
    private void FixedUpdate()
    {
        if (_targetPlayer != null && !_playerInRange)
        {
            _agent.SetDestination(_targetPlayer.position);

            Vector3 moveDir = _agent.velocity.normalized;
            if (moveDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }
    public void ApplyStats()
    {
        _currentHP = _stats.HP;
        UpdateColorEnemyByLevel();
    }
    private IEnumerator WaitForPlayer()
    {
        while(_targetPlayer == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                _targetPlayer = foundPlayer.transform;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void CheckTypeAttack()
    {
        if (_targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, _targetPlayer.position);
        _playerInRange = distance <= dataEnemy.AttackRange;

        if (_playerInRange && _targetPlayer != null)
        {
            Vector3 lookDir = (_targetPlayer.position - transform.position).normalized;
            lookDir.y = 0f;
            transform.rotation = Quaternion .LookRotation(lookDir);
        }
        if (_playerInRange)
        {
            if (!_isAttacking && _cooldown <= 0)
            {
                switch (dataEnemy.TypeEnemy)
                {
                    case TypeEnemy.normal:
                    case TypeEnemy.Explosion:
                        StartCoroutine(AttackMeleeRoutine());
                        break;
                    case TypeEnemy.ranger:
                        StartCoroutine(AttackRangerRoutine());
                        break;
                    case TypeEnemy.boss:
                        StartCoroutine(AttackBossRoutine());
                        break;
                }
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            ResumeMoving();
        }

        _cooldown -= Time.deltaTime;
    }

    private IEnumerator AttackMeleeRoutine()
    {
        _isAttacking = true;
        StopMoving();

        AttackResult result = dataEnemy.AttackStrategy.EnemyAttack(transform, _targetPlayer, _stats.Damage);

        if (result != null && result.target != null)
        {
            IDamageable damageable = result.target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(result.damage);
            }
        }
        _cooldown = dataEnemy.AttackCooldown;

        yield return new WaitForSeconds(1f);
        _isAttacking = false;
    }
    private IEnumerator AttackRangerRoutine()
    {
        _isAttacking = true;
        StopMoving();

        dataEnemy.AttackStrategy.EnemyAttack(transform, _targetPlayer, _stats.Damage);

        _cooldown = dataEnemy.AttackCooldown;

        yield return new WaitForSeconds(1f);
        _isAttacking = false;
    }
    private IEnumerator AttackBossRoutine()
    {
        _isAttacking = true;
        StopMoving();

        dataEnemy.AttackStrategy.EnemyAttack(transform, _targetPlayer, _stats.Damage);

        _cooldown = dataEnemy.AttackCooldown;

        yield return new WaitForSeconds(1f);
        _isAttacking = false;
    }

    public void StartExlosionAttackCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    private void StopMoving()
    {
        if (_agent.enabled && _agent.isActiveAndEnabled)
            _agent.isStopped = true;
    }

    private void ResumeMoving()
    {
        if (_agent.enabled && _agent.isActiveAndEnabled)
            _agent.isStopped = false;
    }
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0 && IsAlive)
            Die();
    }
    private void UpdateColorEnemyByLevel()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        Color color = _stats.Level switch
        {
            LevelEnemy.Green => Color.green,
            LevelEnemy.Blue => Color.blue,
            LevelEnemy.Violet => new Color(0.5f, 0f, 1f),
            LevelEnemy.Yellow => Color.yellow,
            LevelEnemy.Orange => new Color(1f, 0.5f, 0f),
            LevelEnemy.Red => Color.red,
            _ => Color.white
        };

        renderer.material.color = color;
    }
    private void Die()
    {
        if (!IsAlive) return;

        IsAlive = false;
        OnEnemyDie?.Invoke(this);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            int exp = _stats.ExpReward;
            player.AddXP(exp);
        }

        MultiObjectPool.Instance.ReturnToPool(_enemyKey, gameObject);
        EnemyLoot loot = GetComponent<EnemyLoot>();
        loot?.DropLoot();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataEnemy?.AttackRange ?? 1f);
    }
    
}
