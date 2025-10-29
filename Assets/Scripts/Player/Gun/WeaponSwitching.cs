using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private Transform _gunParent;
    [SerializeField] private SwitchImage _switchImage;
    private int _currentIndex = 0;
    [SerializeField] private List<GunController> _equippedGuns;

    public GunController CurrentGun
    {
        get
        {
            if (_currentIndex >= 0 && _currentIndex < _equippedGuns.Count)
            {
                return _equippedGuns[_currentIndex];
            }
            return null;
        }
    }

    private void Awake()
    {
        if (_gunParent == null)
        {
            var gunSlots = transform.Find("Head/Gun");
            if (gunSlots != null)
            {
                _gunParent = gunSlots;
            }
            else
            {
                Debug.LogError("Không tìm thấy GunSlots trong Player!");
            }
        }

        _equippedGuns = new List<GunController>(_gunParent.childCount);

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
        GetGunListToUpdateUI();
        _currentIndex = 0;
        ShowWeapon(_currentIndex);
            
    }
    private void Update()
    {
        HandleNumberKeyInput();
        HandleScrollInput();
    }
    private void GetGunListToUpdateUI()
    {
        List<GunType> gunTypes = new List<GunType>();
        foreach (var gun in _equippedGuns)
        {
            if (gun != null)
                gunTypes.Add(gun.GunType);
            else
                gunTypes.Add(GunType.None);
        }

        _switchImage.GenerateIconGunsByType(gunTypes);
    }
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
    public void ShowWeapon(int index)
    {
        for (int i = 0; i < _gunParent.childCount; i++)
        {
            Transform slot = _gunParent.GetChild(i);
            for (int j = 0; j < slot.childCount; j++)
            {
                slot.GetChild(j).gameObject.SetActive(i == index);
            }
        }

        _currentIndex = index;
        _switchImage.UpdateImageUI(index);

        if (index >= 0 && _switchImage != null)
            _switchImage.UpdateImageUI(index);

        WeaponEvents.OnWeaponChanged?.Invoke(CurrentGun);
    }
    public void SpawnAndEquipWeapon(int slotIndex, GunAttributes gunAttributes, bool showImmediately = false)
    {
        if (_gunParent == null) return;
        if (slotIndex < 0 || slotIndex >= _gunParent.childCount) return;

        Transform slotTransform = _gunParent.GetChild(slotIndex);

        foreach (Transform child in slotTransform)
            Destroy(child.gameObject);

        while (_equippedGuns.Count <= slotIndex)
            _equippedGuns.Add(null);

        _equippedGuns[slotIndex] = null;

        if (gunAttributes == null || gunAttributes.GunPrefab == null)
        {
            GetGunListToUpdateUI();
            return;
        }

        var gunInstance = Instantiate(gunAttributes.GunPrefab, slotTransform);

        gunInstance.transform.localPosition = gunAttributes.PositionOffset;
        gunInstance.transform.localEulerAngles = gunAttributes.RotationOffset;
        gunInstance.transform.localScale = gunAttributes.ScaleOffset;

        var gunController = gunInstance.GetComponent<GunController>();
        _equippedGuns[slotIndex] = gunController;

        if (_switchImage != null)
            GetGunListToUpdateUI();

        if (showImmediately)
            ShowWeapon(slotIndex);
    }
}
