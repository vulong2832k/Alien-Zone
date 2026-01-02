using System;
using System.Collections.Generic;
using UnityEngine;
public class WeaponSwitching : MonoBehaviour
{
    public static WeaponSwitching Instance;

    [Header("Gun Setup")]
    [SerializeField] private Transform _gunParent;
    [SerializeField] private SwitchImage _switchImage;
    private List<GunController> _equippedGuns = new List<GunController>();
    private int _currentIndex = 0;
    public GunController CurrentGun { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (_gunParent == null)
        {
            Transform gunSlots = transform.Find("Head/Gun");
            if (gunSlots != null)
                _gunParent = gunSlots;
        }

        _equippedGuns.Clear();
        for (int i = 0; i < _gunParent.childCount; i++)
        {
            var slot = _gunParent.GetChild(i);
            var gun = slot.GetComponentInChildren<GunController>(true);
            _equippedGuns.Add(gun);
        }

        if (_switchImage == null)
            _switchImage = FindAnyObjectByType<SwitchImage>();
    }

    private void Start()
    {
        _currentIndex = 0;
        ShowWeapon(_currentIndex);
        UpdateUI();
    }

    private void Update()
    {
        HandleNumberKeyInput();
        HandleScrollInput();
    }
    #region Input
    private void HandleNumberKeyInput()
    {
        if (_equippedGuns.Count == 0) return;

        int maxKeys = 4;
        int count = Mathf.Min(_equippedGuns.Count, maxKeys);

        for (int i = 0; i < count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ShowWeapon(i);
                break;
            }
        }
    }
    private void HandleScrollInput()
    {
        if (_equippedGuns.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            int nextIndex = (_currentIndex + 1) % _equippedGuns.Count;
            ShowWeapon(nextIndex);
        }
        else if (scroll < 0f)
        {
            int prevIndex = (_currentIndex - 1 + _equippedGuns.Count) % _equippedGuns.Count;
            ShowWeapon(prevIndex);
        }
    }
    #endregion

    #region Weapon Functions
    public void ShowWeapon(int index)
    {
        if (_equippedGuns.Count == 0) return;
        if (index < 0 || index >= _equippedGuns.Count) return;

        foreach (var gun in _equippedGuns)
            if (gun != null)
                gun.gameObject.SetActive(false);

        // Bật đúng súng
        CurrentGun = _equippedGuns[index];
        if (CurrentGun != null)
            CurrentGun.gameObject.SetActive(true);

        _currentIndex = index;

        WeaponEvents.RaiseWeaponChanged(CurrentGun);
        UpdateUI();
    }

    public GunController SpawnAndEquipWeapon(int slotIndex, GunAttributes gunAttributes, bool showImmediately = false)
    {
        if (_gunParent == null) return null;
        if (slotIndex < 0 || slotIndex >= _gunParent.childCount) return null;

        Transform slot = _gunParent.GetChild(slotIndex);

        foreach (Transform child in slot)
            Destroy(child.gameObject);

        GunController gunController = null;
        if (gunAttributes != null && gunAttributes.GunPrefab != null)
        {
            var gunInstance = Instantiate(gunAttributes.GunPrefab, slot);
            gunInstance.transform.localPosition = gunAttributes.PositionOffset;
            gunInstance.transform.localEulerAngles = gunAttributes.RotationOffset;
            gunInstance.transform.localScale = gunAttributes.ScaleOffset;

            gunController = gunInstance.GetComponent<GunController>();
        }

        if (_equippedGuns.Count > slotIndex)
            _equippedGuns[slotIndex] = gunController;
        else
        {
            while (_equippedGuns.Count <= slotIndex)
                _equippedGuns.Add(null);
            _equippedGuns[slotIndex] = gunController;
        }

        if (showImmediately)
            ShowWeapon(slotIndex);
        else
        {
            CurrentGun = _equippedGuns[slotIndex];
            WeaponEvents.RaiseWeaponChanged(CurrentGun);
            UpdateUI();
        }

        return gunController;
    }

    public void SwitchGun(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _equippedGuns.Count) return;

        ShowWeapon(slotIndex);
    }

    private void UpdateUI()
    {
        if (_switchImage == null) return;

        List<GunType> gunTypes = new List<GunType>();
        foreach (var gun in _equippedGuns)
        {
            gunTypes.Add(gun != null ? gun.GunType : GunType.None);
        }
        _switchImage.GenerateIconGunsByType(gunTypes);
        _switchImage.UpdateImageUI(_currentIndex);
    }
    #endregion
}
