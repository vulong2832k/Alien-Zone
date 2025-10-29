using UnityEngine;

public enum GunType
{
    None = 0,
    Pistol,
    Shotgun,
    Rifle,
    Sniper,
    SMG,
    MG,
    Universal,
    RPG
}

public enum BulletType
{
    Normal,
    Missile,
    Grenade
}

[CreateAssetMenu(fileName = "GunAttributes", menuName = "GunSO/GunAttributes")]
public class GunAttributes : ScriptableObject
{
    [Header("Stats All: ")]
    [SerializeField] private string _name;
    [SerializeField] private int _damage;
    [SerializeField] private int _ammo;
    [SerializeField] private int _reserveAmmo;
    [SerializeField] private int _maxAmmoReserve;
    [SerializeField] private float _fireSpeed;
    [SerializeField] private float _reload;
    [SerializeField] private GameObject _gunPrefab;
    [SerializeField] private AmmoSO _requiredAmmo;

    [Header("Stats Bonus Type Shotgun: ")]
    [SerializeField] private int _quantityPerShoot;
    [SerializeField] private float _speardAngle;

    [Header("Types: ")]
    [SerializeField] private GunType _gunType;
    [SerializeField] private BulletType _bulletType;

    [Header("Equip Offsets (local to slot)")]
    [SerializeField] private Vector3 _positionOffset = Vector3.zero;
    [SerializeField] private Vector3 _rotationOffset = Vector3.zero;
    [SerializeField] private Vector3 _scaleOffset = Vector3.one;

    // --- Properties ---
    public string Name => _name;
    public int Damage => _damage;
    public int Ammo => _ammo;
    public int ReserveAmmo => _reserveAmmo;
    public int MaxAmmo => _maxAmmoReserve;

    public float FireSpeed => _fireSpeed;
    public float Reload => _reload;
    public GunType GunType => _gunType;
    public BulletType BulletType => _bulletType;

    public GameObject GunPrefab => _gunPrefab;
    public AmmoSO RequiredAmmo => _requiredAmmo;

    // Type Shotgun
    public int QuantityPerShoot => _quantityPerShoot;
    public float SpeardAngle => _speardAngle;

    // Offsets
    public Vector3 PositionOffset => _positionOffset;
    public Vector3 RotationOffset => _rotationOffset;
    public Vector3 ScaleOffset => _scaleOffset;
}
