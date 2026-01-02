using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController Instance { get; private set; }

    public System.Action OnPlayerDead;

    [SerializeField] private PlayerStats _stats;
    [SerializeField] private PlayerLevelSystem _levelSystem;

    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private float _healRecoveryBasic;
    [SerializeField] private float _healRecoveryBonus;
    [SerializeField] private float _healRecoveryTotal;
    public int MaxHP => _maxHP;
    public int CurrentHP => _currentHP;

    public bool IsDead { get; private set; }

    [Header("Move: ")]
    [SerializeField] private float _moveSpeed = 5f;
    private float _defaultMoveSpeed;
    public Vector2 MoveInput { get; private set; }
    public float MoveSpeed {  get; set; }
    public bool IsMovingBackward => MoveInput.y < -0.1f;
    public float DefaultMoveSpeed => _defaultMoveSpeed;
    private float _currentYVelocity;
    public float smoothTime = 0.25f;

    [Header("Walking:")]
    [SerializeField] private float _walkSpeed = 3f;
    public float WalkSpeed => _walkSpeed;
    public float MoveWalking { get; set; }

    public bool WantWalk => Input.GetKey(KeyCode.LeftShift);

    [Header("Jump: ")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.3f;

    public bool JumpPressed { get; private set; }

    public float JumpForce => _jumpForce;

    private bool _isGrounded;

    [Header("Crouch: ")]
    [SerializeField] private Transform _body;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _crouchYScale;
    private float _startYScale;

    public float CrouchSppeed => _crouchSpeed;
    public float CrouchYScale => _crouchYScale;

    private CapsuleCollider _collider;
    public bool IsCrouching { get; set; }

    [Header("Gun: ")]
    public GunController Gun => _weaponSwitching.CurrentGun;
    [SerializeField] private WeaponSlots[] _weaponSlots;

    private GunStateType _currentGunStateType = GunStateType.Global;

    public bool ReloadPressed;

    [Header("References:")]
    [SerializeField] private Transform _cameraTransform;
    public Transform CameraTransform => _cameraTransform;

    [SerializeField] private WeaponSwitching _weaponSwitching;

    private float _turnSmoothVelocity;  

    private GunStateMachine _gunStateMachine;

    public InventorySystem Inventory { get; private set; }

    [Header("Coroutine: ")]
    private Coroutine _healRoutine;

    [Header("StateMachine: ")]
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    public PlayerCrouchWalkState CrouchWalkState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerPickState PickState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }

    [Header("Action StateMachine: ")]
    public PlayerActionStateMachine ActionStateMachine;
    public PlayerNoneActionState NoneActionState;
    public PlayerReloadState ReloadState;

    public bool IsActionLocked;

    [Header("Interactable: ")]
    public IInteractable CurrentInteractable { get; private set; }

    [Header("Componenets: ")]
    public Rigidbody PlayerRb { get; private set; }

    private Animator _animator;
    public Animator Animator => _animator;

    //Event
    public event Action<int, int> OnHealthChanged;
    public event Action<int, int, int> OnXPChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_stats == null)
            _stats = GetComponent<PlayerStats>();

        if (_levelSystem == null)
            _levelSystem = GetComponent<PlayerLevelSystem>();

        if (_stats == null)
            Debug.LogError("❌ PlayerStats is MISSING on Player!");

        if (_levelSystem == null)
            Debug.LogError("❌ PlayerLevelSystem is MISSING on Player!");

        if (_weaponSwitching == null)
            _weaponSwitching = FindAnyObjectByType<WeaponSwitching>();

        InitAttributes();
        GetComponentWhenStart();
        InitActionState();

        WeaponEvents.OnWeaponChanged += OnGunChanged;
    }
    void Start()
    {
        _maxHP = _stats.MaxHP;
        _currentHP = _maxHP;

        InitStateMachine();

        _stats.OnStatsChanged += ApplyStatsFromStatsSystem;
        _levelSystem.OnXPChanged += HandleXPChanged;

        _healRoutine = StartCoroutine(HealOverTime());

        OnHealthChanged?.Invoke(_currentHP, _maxHP);
    }

    private void Update()
    {
        GetInputValue();
        ActionStateMachine.CurrentState.Update();

        if (!IsActionLocked)
        {
            StateMachine.CurrentState.HandleInput();
        }
        StateMachine.CurrentState.Update();

        HandleKeyboardInput();
    }
    void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdate();
    }

    private void InitAttributes()
    {
        _defaultMoveSpeed = _moveSpeed;
        MoveSpeed = _moveSpeed;
        MoveWalking = _moveSpeed * 0.4f;

        if (_cameraTransform == null)
            _cameraTransform = Camera.main.transform;

        _startYScale = transform.localScale.y;
    }
    private void GetComponentWhenStart()
    {
        PlayerRb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _weaponSwitching = FindAnyObjectByType<WeaponSwitching>();
        _animator = GetComponentInChildren<Animator>();
    }
    #region State Pattern
    private void InitActionState()
    {
        ActionStateMachine = new PlayerActionStateMachine();

        NoneActionState = new PlayerNoneActionState(this, ActionStateMachine);
        ReloadState = new PlayerReloadState(this, ActionStateMachine);

        ActionStateMachine.Initialize(NoneActionState);
    }
    private void InitStateMachine()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        CrouchWalkState = new PlayerCrouchWalkState(this, StateMachine);
        WalkState =  new PlayerWalkState(this, StateMachine);
        PickState =  new PlayerPickState(this, StateMachine);
        DeadState =  new PlayerDeadState(this, StateMachine);

        StateMachine.Initialize(IdleState);
    }
    public void PlayGunBasedAnimation(string global, string pistol, string rifle)
    {
        if (Gun == null)
        {
            Animator.Play(global);
            return;
        }

        switch (_currentGunStateType)
        {
            case GunStateType.Pistol:
                Animator.Play(pistol);
                break;
            case GunStateType.Rifle:
                Animator.Play(rifle);
                break;
            default:
                Animator.Play(global);
                break;
        }
    }
    private void RefreshCurrentStateAnimation()
    {
        if (StateMachine == null || StateMachine.CurrentState == null)
            return;

        StateMachine.CurrentState.OnGunChanged();
    }
    #endregion
    #region Event
    private void OnDestroy()
    {
        WeaponEvents.OnWeaponChanged -= OnGunChanged;

        if (_stats != null)
            _stats.OnStatsChanged -= ApplyStatsFromStatsSystem;

        if (_levelSystem != null)
            _levelSystem.OnXPChanged -= HandleXPChanged;
    }

    private void OnGunChanged(GunController newGun)
    {
        if (newGun != null && newGun.GunAttributes != null)
        {
            _currentGunStateType = newGun.GunAttributes.StateType;
        }
        else
        {
            _currentGunStateType = GunStateType.Global;
        }

        RefreshCurrentStateAnimation();
    }
    #endregion
    #region LevelSystem
    private void ApplyStatsFromStatsSystem()
    {
        if (_stats == null) return;

        _maxHP = _stats.MaxHP;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        MoveSpeed = _defaultMoveSpeed * _stats.MoveSpeedMultiplier;

        OnHealthChanged?.Invoke(_currentHP, _maxHP);
    }

    private void HandleXPChanged(int currentXP, int xpToNext, int level)
    {
        OnXPChanged?.Invoke(currentXP, xpToNext, level);
    }
    public void GainXP(int amount)
    {
        _levelSystem.AddXP(amount, _stats.ExpMultiplier);
    }
    #endregion
    private void GetInputValue()
    {
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        IsCrouching = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.R) && Gun != null && Gun.CanReload())
        {
            ReloadPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetJumpPressed();
        }
    }
    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryUseMedicine();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TryUseAmmo();
        }
        if (Input.GetKeyDown(KeyCode.E) && CurrentInteractable != null)
        {
            StateMachine.ChangeState(PickState);
        }
        
    }
    public void PlayerMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 inputMove = forward * MoveInput.y + right * MoveInput.x;

        if (inputMove.sqrMagnitude < 0.0001f)
        {
            PlayerRb.linearVelocity = new Vector3(0f, PlayerRb.linearVelocity.y, 0f);
            return;
        }

        Vector3 targetVel = inputMove.normalized * MoveSpeed;
        Vector3 newVel = Vector3.Lerp(PlayerRb.linearVelocity, new Vector3(targetVel.x, PlayerRb.linearVelocity.y, targetVel.z), 0.2f);
        PlayerRb.linearVelocity = newVel;
    }
    public void RotateToCameraForwardSmooth()
    {
        Vector3 lookDir = CameraTransform.forward;
        lookDir.y = 0;
        transform.DORotateQuaternion(Quaternion.LookRotation(lookDir), 0.1f);
    }
    public void RotateToCameraDirection()
    {
        if (MoveInput.y < 0f) return;

        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        float targetYaw = Mathf.Atan2(camForward.x, camForward.z) * Mathf.Rad2Deg;
        float newYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetYaw, ref _currentYVelocity,smoothTime);

        transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
    }

    public void TweenCrouch(bool isCrouching)
    {
        float targetY = isCrouching ? _crouchYScale : _startYScale;
        _body.DOScaleY(targetY, 0.2f).SetEase(Ease.InOutSine);
        MoveSpeed = isCrouching ? _crouchSpeed : _defaultMoveSpeed;
    }
    public void TweenCrouchCollider(bool isCrouching)
    {
        if (_collider == null) return;

        float targetHeight = isCrouching ? _crouchYScale : _startYScale;
        float targetCenterY = targetHeight / 2f;

        DOTween.To(() => _collider.height, x => _collider.height = x, targetHeight, 0.2f).SetEase(Ease.InOutSine);
        DOTween.To(() => _collider.center, x => _collider.center = x,
            new Vector3(_collider.center.x, targetCenterY, _collider.center.z), 0.2f).SetEase(Ease.InOutSine);
    }

    public void SetCrouch(bool isCrouching)
    {
        IsCrouching = isCrouching;
        TweenCrouch(isCrouching);
        TweenCrouchCollider(isCrouching);
    }
    public bool IsGrounded()
    {
        if (_groundCheck == null) return false;
        return Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }
    public void SetJumpPressed()
    {
        this.JumpPressed = true;
    }
    public void ConsumeJumpPressed()
    {
        this.JumpPressed = false;
    }
    public void OnWeaponEquipped(GunAttributes gun)
    {
        if (_gunStateMachine == null)
            _gunStateMachine = new GunStateMachine();
    }

    public GunStateType GetCurrentGunStateType() => _currentGunStateType;

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        GameManager.Instance.AddDamageTaken(damage);
        OnHealthChanged?.Invoke(_currentHP, _maxHP);

        ScreenEffectManager.Instance.Play(ScreenEffectType.Hurt, 0.15f);

        if (_currentHP <= 0)
        {
            IsDead = true;
            IsActionLocked = true;

            OnPlayerDead?.Invoke();

            Time.timeScale = 0.3f;

            ScreenEffectManager.Instance.Play(ScreenEffectType.Death);

            if (_healRoutine != null)
                StopCoroutine(_healRoutine);

            StateMachine.ChangeState(DeadState);
        }
    }

    private IEnumerator HealOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            _healRecoveryTotal = _healRecoveryBasic * (1f + _stats.hpPoint * 0.05f);
            if (_currentHP < _maxHP && _healRecoveryTotal > 0)
            {
                _currentHP += Mathf.RoundToInt(_healRecoveryTotal);
                _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
                OnHealthChanged?.Invoke(_currentHP, _maxHP);
            }
        }
    }
    #region Gun
    public void AddAmmo(AmmoSO ammo)
    {
        if (_weaponSwitching == null || ammo == null) return;

        GunController currentGun = _weaponSwitching.CurrentGun;
        if (currentGun == null) return;

        int maxReserve = currentGun.GunAttributes.MaxAmmo;
        float randomPercent = Random.Range(ammo.minPercentRecover, ammo.maxPercentRecover);
        int addAmount = Mathf.Max(1, Mathf.RoundToInt(currentGun.GunAttributes.MaxAmmo * randomPercent));

        currentGun.AddReserveAmmo(addAmount);
        WeaponEvents.OnWeaponChanged?.Invoke(currentGun);
    }

    private void TryUseAmmo()
    {
        var slots = EquipmentSystem.Instance.GetAllSlots();

        foreach (var slot in slots)
        {
            if (slot.AllowedType == ItemType.Ammo && !slot.IsEmpty)
            {
                var ammoSO = slot.GetItem() as AmmoSO;
                if (ammoSO != null)
                {
                    AddAmmo(ammoSO);
                    slot.ReduceItem(1);
                    break;
                }
            }
        }
    }
    #endregion
    #region UseItem
    private void UseMedicine(MedicineSO medicineSO, EquipmentSlotUI slotUI)
    {
        if (medicineSO == null || _currentHP >= _maxHP) return;

        _currentHP += medicineSO.recoveryHP;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        OnHealthChanged?.Invoke(_currentHP, _maxHP);

        slotUI.ReduceItem(1);
    }
    private void TryUseMedicine()
    {
        var slots = EquipmentSystem.Instance.GetAllSlots();

        foreach (var slot in slots)
        {
            if (slot.AllowedType == ItemType.Medicine && !slot.IsEmpty)
            {
                var medicineSO = slot.GetItem() as MedicineSO;
                if (medicineSO != null)
                {
                    UseMedicine(medicineSO, slot);
                    break;
                }
            }
        }
    }

    #endregion
    #region Interactable
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            CurrentInteractable = other.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            CurrentInteractable = null;
        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
