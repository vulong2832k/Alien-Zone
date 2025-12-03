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
    public Transform Target => _targetPlayer;

    [SerializeField] private string _enemyKey = "Enemy";

    [SerializeField] private EnemyStats _stats;
    public EnemyStats stats => _stats;

    [Header("Components: ")]
    public Animator anim;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [Header("Logic: ")]
    private bool _isAttacking = false;
    private bool _playerInRange = false;
    public bool IsAlive { get; private set; } = true;

    [Header("StateMachine: ")]
    public EnemyStateMachine stateMachine;

    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyHurtState HurtState { get; private set; }
    public EnemyDeathState DeathState { get; private set; }

    //Event
    public event Action<EnemyController> OnEnemyDie;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _stats = GetComponent<EnemyStats>();

        InitStateMachine();

        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
            _targetPlayer = player.transform;
    }

    private void OnEnable()
    {
        _cooldown = 0f;
        _isAttacking = false;
        _playerInRange = false;
        StartCoroutine(InitAfterSpawn());

        if (dataEnemy != null && dataEnemy.AttackStrategy != null)
            AttackState = new EnemyAttackState(this, stateMachine, dataEnemy.AttackStrategy);
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(WaitForPlayer());
        stateMachine.Initialize(IdleState);

        if (dataEnemy != null && _agent != null)
            _agent.speed = dataEnemy.MoveSpeed;

        ApplyStats();
    }

    private void Update()
    {
        if (stateMachine?.CurrentState != null)
            stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        if (stateMachine?.CurrentState != null)
            stateMachine.CurrentState.PhysicsUpdate();
    }
    private IEnumerator InitAfterSpawn()
    {
        yield return null;
        if (_agent != null && _agent.isOnNavMesh)
        {
            _agent.isStopped = false;
        }
    }
    #region StateMachine
    private void InitStateMachine()
    {
        stateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, stateMachine);
        MoveState = new EnemyMoveState(this, stateMachine);
        HurtState = new EnemyHurtState(this, stateMachine);
        DeathState = new EnemyDeathState(this, stateMachine);
    }
    public void PlayAnim(string nameState)
    {
        if (anim != null)
            anim.Play(nameState);
    }
    public bool HasAnimation(string animName)
    {
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
                return true;
        }
        return false;
    }
    #endregion
    public void StopMoving()
    {
        if (_agent != null && _agent.enabled && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
    }
    public void ResumeMoving()
    {
        if (_agent != null && _agent.enabled && _agent.isOnNavMesh)
            _agent.isStopped = false;
    }
    #region AttackType
    public void OnAttackHit()
    {
        if (!IsAlive || Target == null || dataEnemy == null)
            return;

        var result = dataEnemy.AttackStrategy?.EnemyAttack(transform, Target, _stats.Damage);
        if (result == null) return;

        if (dataEnemy.AttackStrategy is SuicideAttackSO suicideSO)
        {
            DoExplosionDamage(suicideSO.ExplosionRadius, result.damage);

            SpawnExplosionEffect(suicideSO.ExplosionEffectKey);

            DieFromExplosion();
            return;
        }

        if (dataEnemy.AttackStrategy is RangerAttackSO rangerSO)
        {
            FireBullet(rangerSO.BulletKey);
            return;
        }

        if (dataEnemy.AttackStrategy is MissileAttackSO missileSO)
        {
            FireMissile(missileSO.MissileKey);
            return;
        }

        var dmgable = result.target.GetComponent<IDamageable>();
        dmgable?.TakeDamage(result.damage);
    }

    public void OnAttackEvent()
    {
        OnAttackHit();
    }
    public void FaceTarget()
    {
        if (Target == null) return;

        Vector3 dir = Target.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);
        }
    }
    //------------------------------------------Enemy Explosion---------------------------------------------------------
    public void DoExplosionDamage(float radius, int damage)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
            dmg?.TakeDamage(damage);
        }
    }
    public void SpawnExplosionEffect(string key)
    {
        GameObject explosionEff = MultiObjectPool.Instance.SpawnFromPool(key, transform.position, Quaternion.identity);

        ParticleSystem particleSystem = explosionEff.GetComponent<ParticleSystem>();
        float total = (particleSystem != null)
            ? particleSystem.main.duration + particleSystem.main.startLifetime.constantMax : 1.5f;

        StartCoroutine(ReturnExplosionEffect(explosionEff, total));
    }
    private IEnumerator ReturnExplosionEffect(GameObject explosionEffect, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        MultiObjectPool.Instance.ReturnToPool("ExplosionEffect", explosionEffect);
    }
    public void DieFromExplosion()
    {
        MultiObjectPool.Instance.ReturnToPool(_enemyKey, gameObject);
    }
    //------------------------------------------Enemy Missiler-------------------------------------------------------------
    public void FireMissile(string bulletKey)
    {
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint == null) return;
        
        Vector3 direction = (Target.position - firePoint.position).normalized;

        GameObject bulletMissile = MultiObjectPool.Instance.SpawnFromPool(bulletKey, firePoint.position, Quaternion.LookRotation(direction));

        if (bulletMissile.TryGetComponent(out EnemyBulletBase script))
        {
            script.Init(direction, stats.Damage);
        }   
    }
    //------------------------------------------Enemy Ranger-------------------------------------------------------------
    public void FireBullet(string bulletKey)
    {
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint == null) return;

        Vector3 direction = (Target.position - firePoint.position).normalized;

        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(bulletKey, firePoint.position, Quaternion.LookRotation(direction));

        if (bullet.TryGetComponent(out EnemyBulletBase script))
        {
            script.Init(direction, stats.Damage);
        }
    }
    #endregion
    public void ApplyStats()
    {
        if (_stats != null)
            _currentHP = _stats.HP;

        UpdateColorEnemyByLevel();
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;
        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            Die();
        }
        else
        {
            stateMachine.ChangeState(HurtState);
        }
    }

    private void Die()
    {
        if (!IsAlive) return;

        IsAlive = false;

        OnEnemyDie?.Invoke(this);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null && _stats != null)
            player.AddXP(_stats.ExpReward);

        EnemyLoot loot = GetComponent<EnemyLoot>();
        loot?.DropLoot();

        stateMachine.ChangeState(DeathState);
    }
    public void OnDeathComplete()
    {
        MultiObjectPool.Instance.ReturnToPool(_enemyKey, gameObject);
    }

    private IEnumerator WaitForPlayer()
    {
        while (_targetPlayer == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                _targetPlayer = foundPlayer.transform;

            yield return new WaitForSeconds(0.1f);
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dataEnemy?.AttackRange ?? 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, dataEnemy?.FollowRange ?? 0f);
    }
    
}
