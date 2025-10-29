using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour, IAmmoObserver
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _ammoImage;

    private GunController _gun;

    private void OnEnable()
    {
        WeaponEvents.OnWeaponChanged += OnWeaponChanged;
    }
    private void OnDisable()
    {
        WeaponEvents.OnWeaponChanged -= OnWeaponChanged;
        UnsubscribeFromGun();
    }
    private void Awake()
    {
        if (_ammoText == null)
        {
            GameObject ammoTextObj = GameObject.Find("AmmoText");
            if (ammoTextObj != null)
                _ammoText = ammoTextObj.GetComponent<TextMeshProUGUI>();
        }

        if (_ammoImage == null)
        {
            GameObject ammoImgObj = GameObject.Find("AmmoImage");
            if (ammoImgObj != null)
                _ammoImage = ammoImgObj.GetComponent<Image>();
        }
    }
    private void OnWeaponChanged(GunController newGun)
    {
        UnsubscribeFromGun();

        _gun = newGun;

        if (_gun != null)
        {
            _gun.OnAmmoChanged += OnAmmoChanged;
            OnAmmoChanged(_gun.CurrentAmmo, _gun.ReserveAmmo, false);
            if (_ammoImage != null) _ammoImage.fillAmount = _gun.FireCooldownNormalized;
        }
        else
        {
            UpdateUI(0, 0, false);
            if (_ammoImage != null) _ammoImage.fillAmount = 0f;
        }
    }
    private void UnsubscribeFromGun()
    {
        if (_gun != null)
        {
            _gun.OnAmmoChanged -= OnAmmoChanged;
            _gun = null;
        }
    }
    public void OnAmmoChanged(int currentAmmo, int reserveAmmo, bool isReloading)
    {
        UpdateUI(currentAmmo, reserveAmmo, isReloading);

        if (_ammoImage != null && _gun != null)
        {
            _ammoImage.fillAmount = _gun.FireCooldownNormalized;
        }
    }
    private void UpdateUI(int currentAmmo, int reserveAmmo, bool isReloading)
    {
        if (_ammoText == null) return;

        if (isReloading)
        {
            _ammoText.text = "Reloading...";
            _ammoText.color = Color.yellow;
        }
        else
        {
            _ammoText.text = $"{currentAmmo:D3} / {reserveAmmo:D3}";
            _ammoText.color = (currentAmmo == 0 && reserveAmmo == 0) ? Color.red : Color.white;
        }
    }
    
}
