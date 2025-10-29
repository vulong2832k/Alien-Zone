using UnityEngine;

public class GrenadeSwitcher : MonoBehaviour
{
    [SerializeField] private Transform _handTransform;
    [SerializeField] private GameObject _grenadePrefab;
    [SerializeField] private WeaponSwitching _weaponSwitching;

    private GameObject _currentGrenade;
    private bool _isGrenadeActive = false;

    private void Start()
    {
        if(_weaponSwitching == null)
            _weaponSwitching = FindAnyObjectByType<WeaponSwitching>();

        if (_grenadePrefab != null)
        {
            _currentGrenade = Instantiate(_grenadePrefab, _handTransform);
            _currentGrenade.transform.localPosition = Vector3.zero;
            _currentGrenade.transform.localRotation = Quaternion.identity;
            _currentGrenade.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ToggleGrenade(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) ||
                 Input.GetKeyDown(KeyCode.Alpha2) ||
                 Input.GetKeyDown(KeyCode.Alpha3) ||
                 Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleGrenade(false);
        }
    }
    private void ToggleGrenade(bool active)
    {
        if(_isGrenadeActive == active) return;
        _isGrenadeActive = active;
        
        if (_weaponSwitching != null)
        {
            if (active)
            {
                _weaponSwitching.ShowWeapon(-1);
            }
            else
            {
                _weaponSwitching.ShowWeapon(0);
            }
        }
        if(_currentGrenade != null)
            _currentGrenade.SetActive(active);
    }
    public bool IsGrenadeActive => _isGrenadeActive;
}
