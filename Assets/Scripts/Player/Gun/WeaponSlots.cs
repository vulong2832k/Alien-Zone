using UnityEngine;

public class WeaponSlots : MonoBehaviour
{
    [SerializeField] private Transform _weaponParent;
    private GameObject _currentWeapon;
    public GameObject CurrentGun => _currentWeapon;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon);
        }

        _currentWeapon = Instantiate(weaponPrefab, _weaponParent);
        _currentWeapon.transform.localPosition = Vector3.zero;
        _currentWeapon.transform.localRotation = Quaternion.identity;
    }

    public GameObject GetCurrentWeapon()
    {
        return _currentWeapon;
    }
}
