using System;
using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Refs: ")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GunSOHolder _soHolder;
    [SerializeField] private IShootStrategy _shootStrategy;

    [Header("Bullet Key: ")]
    [SerializeField] private string _bulletKey = "BulletPlayer";

    [Header("Ammo: ")]
    private bool _isReloading = false;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private int _reserveAmmo;
    private float _fireCooldown;

    public bool BlockFire = false;

    public event Action<int, int, bool> OnAmmoChanged;
    public GunAttributes GunAttributes => _soHolder.GunAttributes;
    public GunType GunType => GunAttributes.GunType;

    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => _reserveAmmo;

    [Header("Effects: ")]
    [SerializeField] private GameObject _muzzleFlashEffect;
    private ParticleSystem _muzzleFlashParticle;

    public float FireCooldownNormalized
    {
        get
        {
            if (_soHolder == null || GunAttributes == null || GunAttributes.FireSpeed <= 0f)
                return 0f;
            return Mathf.Clamp01(1f - (_fireCooldown / GunAttributes.FireSpeed));
        }
    }


    private void Awake()
    {
        if (_soHolder == null)
            _soHolder = GetComponentInChildren<GunSOHolder>();

        if (_firePoint == null)
            _firePoint = transform.Find("FirePoint");

        if (_muzzleFlashEffect != null)
            _muzzleFlashParticle = _muzzleFlashEffect.GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (GunAttributes != null)
        {
            (int currentAmmo, int reserveAmmo) = GunDataManager.LoadAmmo(GunAttributes.Name, _soHolder.CurrentAmmo, _soHolder.ReserveAmmo);
            _currentAmmo = currentAmmo;
            _reserveAmmo = reserveAmmo;
        }

        SetTypeShoot();
        NotifyAmmoChanged();
    }

    private void Update()
    {
        AlignFirePointWithCamera();

        HandleShooting();

        if (_fireCooldown > 0)
        {
            float prev = _fireCooldown;
            _fireCooldown -= Time.deltaTime;

            if (_fireCooldown < 0) _fireCooldown = 0f;
            if (Mathf.Abs(prev - _fireCooldown) > 0.0001f)
            {
                NotifyAmmoChanged();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddReserveAmmo(10);
        }
    }

    private void SetTypeShoot()
    {
        switch (GunType)
        {
            case GunType.Shotgun:
                _shootStrategy = new ShotgunShootStrategy();
                break;
            default:
                _shootStrategy = new NormalShootStrategy();
                break;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && CanFire())
        {
            Shooting();
        }
    }

    private bool CanFire() => _fireCooldown <= 0 && !_isReloading && _currentAmmo > 0;

    private void Shooting()
    {
        _fireCooldown = GunAttributes.FireSpeed;
        _currentAmmo--;

        _soHolder.CurrentAmmo = _currentAmmo;
        GunDataManager.SaveAmmo(GunAttributes.Name, _currentAmmo, _reserveAmmo);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 shootDirection;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
            shootDirection = (targetPoint - _firePoint.position).normalized;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
            shootDirection = (targetPoint - _firePoint.position).normalized;
        }

        _firePoint.rotation = Quaternion.LookRotation(shootDirection);

        _shootStrategy?.Shoot(_firePoint, shootDirection, GunAttributes, _bulletKey);

        NotifyAmmoChanged();
        DisplayEffectMuzzleFlash();

        Debug.DrawRay(_firePoint.position, shootDirection * 100f, Color.red, 1f);
    }

    private void AlignFirePointWithCamera()
    {
        if (_firePoint == null || Camera.main == null) return;

        _firePoint.rotation = Quaternion.Lerp(_firePoint.rotation, Camera.main.transform.rotation, Time.deltaTime * 50f);
    }
    private string GetBulletKey(BulletType type)
    {
        switch (type)
        {
            case BulletType.Normal: return "PlayerBulletNormal";
            case BulletType.Missile: return "PlayerBulletMissile";
            case BulletType.Grenade: return "PlayerBulletGrenade";
            default: return "PlayerBulletNormal";
        }
    }
    public bool NeedsReload()
    {
        if (_isReloading) return false;

        return _currentAmmo < GunAttributes.Ammo && _reserveAmmo > 0;
    }
    public bool CanReload()//Check
    {
        if (_isReloading) return false;
        if (CurrentAmmo >= GunAttributes.MaxAmmo) return false;
        if (ReserveAmmo <= 0) return false;
        return true;
    }

    public void DoReload()//Action
    {
        int ammoNeeded = GunAttributes.Ammo - _currentAmmo;

        int toReload = Mathf.Min(ammoNeeded, _reserveAmmo);

        if (toReload > 0)
        {
            _currentAmmo += toReload;
            _reserveAmmo -= toReload;

            _soHolder.CurrentAmmo = _currentAmmo;
            _soHolder.ReserveAmmo = _reserveAmmo;

            GunDataManager.SaveAmmo(GunAttributes.Name, _currentAmmo, _reserveAmmo);
        }
        NotifyAmmoChanged();
    }
    public void TryReload()
    {
        if (!CanReload()) return;

        _isReloading = true;
        NotifyAmmoChanged(true);
        
        StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(GunAttributes.Reload);

        DoReload();

        _isReloading = false;
        NotifyAmmoChanged(false);
    }
    #region UI
    public void AddReserveAmmo(int amount)
    {
        _reserveAmmo += amount;
        _reserveAmmo = Mathf.Clamp(_reserveAmmo, 0, GunAttributes.MaxAmmo);

        _soHolder.ReserveAmmo = _reserveAmmo;

        GunDataManager.SaveAmmo(GunAttributes.Name, _currentAmmo, _reserveAmmo);
        NotifyAmmoChanged();
    }


    private void NotifyAmmoChanged(bool isReloading = false)
    {
        OnAmmoChanged?.Invoke(_currentAmmo, _reserveAmmo, isReloading);
    }

    #endregion

    #region Effect
    private void DisplayEffectMuzzleFlash()
    {
        if (_muzzleFlashParticle != null)
        {
            _muzzleFlashParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _muzzleFlashParticle.Play();
        }
    }
    #endregion
}
