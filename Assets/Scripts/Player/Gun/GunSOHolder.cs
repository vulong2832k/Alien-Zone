using UnityEngine;

public class GunSOHolder : MonoBehaviour
{
    [SerializeField] private GunAttributes _gunAttributes;
    public GunAttributes GunAttributes => _gunAttributes;

    public int CurrentAmmo { get; set; }
    public int ReserveAmmo { get; set; }

    private void Awake()
    {
        if (_gunAttributes != null)
        {
            (CurrentAmmo, ReserveAmmo) = GunDataManager.LoadAmmo(
                _gunAttributes.Name,
                _gunAttributes.Ammo,
                _gunAttributes.ReserveAmmo
            );
        }
    }
}
