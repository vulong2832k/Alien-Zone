using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _cooldownImage;

    [SerializeField] private GunController _currentGun;

    private void OnEnable()
    {
        WeaponEvents.OnWeaponChanged += HandleWeaponChanged;

        var player = PlayerController.Instance;
        if (player != null && player.Gun != null)
        {

        }
            //HandleWeaponChanged(player.Gun);
        else
            ClearUI();
    }

    private void OnDisable()
    {
        WeaponEvents.OnWeaponChanged -= HandleWeaponChanged;
        UnsubscribeGun();
    }

    private void HandleWeaponChanged(GunController newGun)
    {
        UnsubscribeGun();
        _currentGun = newGun;

        if (_currentGun == null)
        {
            ClearUI();
            return;
        }

        _currentGun.OnAmmoChanged += UpdateAmmoUI;

        UpdateAmmoUI(_currentGun.CurrentAmmo, _currentGun.ReserveAmmo, false);
        UpdateCooldownUI();
    }

    private void UnsubscribeGun()
    {
        if (_currentGun != null)
        {
            _currentGun.OnAmmoChanged -= UpdateAmmoUI;
            _currentGun = null;
        }
    }

    private void UpdateAmmoUI(int current, int reserve, bool isReloading)
    {
        if (_ammoText == null) return;

        if (isReloading)
        {
            _ammoText.text = "Reloading...";
            _ammoText.color = Color.yellow;
        }
        else
        {
            _ammoText.text = $"{current:D3} / {reserve:D3}";
            _ammoText.color = (current == 0 && reserve == 0) ? Color.red : Color.white;
        }

        UpdateCooldownUI();
    }

    private void UpdateCooldownUI()
    {
        if (_cooldownImage != null && _currentGun != null)
            _cooldownImage.fillAmount = _currentGun.FireCooldownNormalized;
    }

    private void ClearUI()
    {
        if (_ammoText != null)
            _ammoText.text = "--- / ---";

        if (_cooldownImage != null)
            _cooldownImage.fillAmount = 0f;
    }
}
