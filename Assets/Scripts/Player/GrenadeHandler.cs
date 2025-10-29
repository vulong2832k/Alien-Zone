using UnityEngine;

public class GrenadeHandler : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform _handTransform;
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private GameObject _grenadeViewPrefab;
    [SerializeField] private string _grenadeKey = "Grenade";
    [SerializeField] private WeaponSwitching _weaponSwitching;

    [Header("Throw Settings")]
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private float _upwardForce = 2f;
    [SerializeField] private float _holdThreshold = 0.5f;

    private GameObject _grenadeView;
    private bool _isGrenadeActive;
    private bool _isPullingPin;
    private float _holdTime;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;

        if (_weaponSwitching == null)
            _weaponSwitching = FindAnyObjectByType<WeaponSwitching>();

        if (_grenadeViewPrefab != null && _handTransform != null)
        {
            _grenadeView = Instantiate(_grenadeViewPrefab, _handTransform);
            _grenadeView.transform.localPosition = Vector3.zero;
            _grenadeView.transform.localRotation = Quaternion.identity;
            _grenadeView.SetActive(false);
        }

        if (_throwPoint == null && transform.Find("ThrowGrenade") != null)
            _throwPoint = transform.Find("ThrowGrenade");
    }

    private void Update()
    {
        HandleSlotInput();
        HandleThrowInput();
    }

    private void HandleSlotInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
            ToggleGrenade(true);

        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) ||
            Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleGrenade(false);
        }
    }

    private void HandleThrowInput()
    {
        if (!_isGrenadeActive) return;

        var grenadeSlot = GetGrenadeSlot();
        if (grenadeSlot == null || grenadeSlot.IsEmpty) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isPullingPin = true;
            _holdTime = 0f;
        }

        if (_isPullingPin)
            _holdTime += Time.deltaTime;

        if (Input.GetMouseButtonUp(0))
        {
            if (_holdTime >= _holdThreshold)
            {
                ThrowGrenade();
                grenadeSlot.ReduceItem(1);
            }
            _isPullingPin = false;
        }
    }

    private void ToggleGrenade(bool active)
    {
        if (_isGrenadeActive == active) return;

        _isGrenadeActive = active;

        if (_weaponSwitching != null)
        {
            if (active)
                _weaponSwitching.ShowWeapon(-1);
            else
                _weaponSwitching.ShowWeapon(0);
        }

        if (_grenadeView != null)
            _grenadeView.SetActive(active);
    }

    private EquipmentSlotUI GetGrenadeSlot()
    {
        var slots = EquipmentSystem.Instance.GetAllSlots();

        foreach (var slot in slots)
        {
            if (slot.AllowedType == ItemType.Grenade)
                return slot;
        }
        return null;
    }

    private void ThrowGrenade()
    {
        if (_throwPoint == null) return;

        var grenadeSlot = GetGrenadeSlot();
        if (grenadeSlot == null || grenadeSlot.IsEmpty) return;

        var grenadeSO = grenadeSlot.GetItem() as GrenadeSO;
        if (grenadeSO == null) return;

        GameObject grenadeObj = MultiObjectPool.Instance.SpawnFromPool(_grenadeKey, _throwPoint.position, Quaternion.identity);
        if (grenadeObj == null) return;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(50f);

        Vector3 dir = (targetPoint - _throwPoint.position).normalized;
        Vector3 force = dir * _throwForce + Vector3.up * _upwardForce;

        if (grenadeObj.TryGetComponent(out Grenade grenade))
        {
            grenade.SetData(grenadeSO);
            grenade.Activate(force, -1f);
        }
        else if (grenadeObj.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }

}
