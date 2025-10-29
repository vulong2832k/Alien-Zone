using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private float _healRecoveryBasic;
    [SerializeField] private float _healRecoveryBonus;
    [SerializeField] private float _healRecoveryTotal;
    public int MaxHP => _maxHP;
    public int CurrentHP => _currentHP;

    [Header("Move: ")]
    [SerializeField] private float _moveSpeed = 5f;
    private float _defaultMoveSpeed;
    public Vector2 MoveInput { get; private set; }
    public float MoveSpeed {  get; set; }
    public float MoveWalking { get; set; }
    public float DefaultMoveSpeed => _defaultMoveSpeed;

    [Header("Jump: ")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.3f;

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

    [Header("Level System: ")]
    private int _currentLevel = 1;
    private int _currentXP = 0;
    [SerializeField] private int _xpToNextLevel = 100;
    [SerializeField] private float _xpGrowthRate = 1.2f;

    [Header("Slot Gun: ")]
    [SerializeField] private WeaponSlots[] _weaponSlots;

    [Header("References:")]
    [SerializeField] private Transform _cameraTransform;
    public Transform CameraTransform => _cameraTransform;
    private WeaponSwitching _weaponSwitching;


    [Header("States: ")]
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    [Header("Componenets: ")]
    public Rigidbody PlayerRb { get; private set; }

    //Event
    public event Action<int, int> OnHealthChanged;
    public event Action<int, int, int> OnXPChanged;

    private void Awake()
    {
        InitAttributes();
        GetComponentWhenStart();
    }

    void Start()
    {
        InitStateMachine();
        OnHealthChanged?.Invoke(_currentHP, _maxHP);
        OnXPChanged?.Invoke(_currentXP, _xpToNextLevel, _currentLevel);
        StartCoroutine(HealOverTime());
    }
    private void Update()
    {
        GetInputValue();
        StateMachine.CurrentState.LogicUpdate();

        HandleKeyboardInput();

        if (Input.GetKeyDown(KeyCode.K)) AddXP(25);
    }
    void FixedUpdate()
    {
        PlayerMovement();
    }
    private void InitAttributes()
    {
        _defaultMoveSpeed = _moveSpeed;
        MoveSpeed = _moveSpeed;
        MoveWalking = _moveSpeed * 0.4f;

        _currentHP = _maxHP;

        _cameraTransform = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;

        if (_groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.SetParent(transform);
            check.transform.localPosition = Vector3.down * 0.85f;
            _groundCheck = check.transform;
        }

        _startYScale = transform.localScale.y;
    }
    private void GetComponentWhenStart()
    {
        PlayerRb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _body = transform.GetChild(1);

        _weaponSwitching = FindAnyObjectByType<WeaponSwitching>();
    }
    private void InitStateMachine()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        CrouchState = new PlayerCrouchState(this, StateMachine);

        StateMachine.Initialize(IdleState);
    }
    private void GetInputValue()
    {
        //Move
        MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Walking

        //Crouch
        IsCrouching = Input.GetKey(KeyCode.LeftControl);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            PlayerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
    }
    public void PlayerMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * MoveInput.y + right * MoveInput.x).normalized;
        Vector3 newVelocity = moveDirection * MoveSpeed;
        newVelocity.y = PlayerRb.linearVelocity.y;
        PlayerRb.linearVelocity = newVelocity;
    }
    public void TweenCrouch(bool isCrouching)
    {
        float targetY = isCrouching ? _crouchYScale : _startYScale;
        _body.DOScaleY(targetY, 0.2f).SetEase(Ease.InOutSine);
        MoveSpeed = isCrouching ? _crouchSpeed : _defaultMoveSpeed;

        float groundCheckYOffset = isCrouching ? -0.5f : -0.85f;
        _groundCheck.DOLocalMoveY(groundCheckYOffset, 0.2f).SetEase(Ease.InOutSine);
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);

        OnHealthChanged?.Invoke(_currentHP, _maxHP);

        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private IEnumerator HealOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            _healRecoveryTotal = _healRecoveryBasic + _healRecoveryBonus;
            if (_currentHP < _maxHP && _healRecoveryTotal > 0)
            {
                _currentHP += Mathf.RoundToInt(_healRecoveryTotal);
                _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
                OnHealthChanged?.Invoke(_currentHP, _maxHP);
            }
        }
    }

    #region LevelSystem
    public void AddXP(int amount)
    {
        _currentXP += amount;

        OnXPChanged?.Invoke(_currentXP, _xpToNextLevel, _currentLevel);
        if (_currentXP >= _xpToNextLevel) LevelUp();
    }
    private void LevelUp()
    {
        _currentXP -= _xpToNextLevel;
        _currentLevel++;

        _xpToNextLevel = Mathf.RoundToInt(_xpToNextLevel * _xpGrowthRate);

        _maxHP += 10;
        _currentHP = _maxHP;

        OnXPChanged?.Invoke(_currentXP, _xpToNextLevel, _currentLevel);
        OnHealthChanged?.Invoke(_currentHP, _maxHP);
    }
    #endregion
    #region Gun
    public void AddAmmo(AmmoSO ammo)
    {
        if (_weaponSwitching == null || ammo == null) return;

        GunController currentGun = _weaponSwitching.CurrentGun;
        if (currentGun == null) return;

        float randomPercent = Random.Range(ammo.minPercentRecover, ammo.maxPercentRecover);
        int addAmount = Mathf.Max(1, Mathf.RoundToInt(currentGun.GunAttributes.MaxAmmo * randomPercent));

        currentGun.AddReserveAmmo(addAmount);
        WeaponEvents.OnWeaponChanged?.Invoke(currentGun);

        Debug.Log($"Đã hồi {addAmount} đạn cho {currentGun.name} ({randomPercent * 100f:F1}%)");
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
    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
