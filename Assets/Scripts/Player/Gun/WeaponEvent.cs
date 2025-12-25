using System;

public static class WeaponEvents
{
    public static Action<GunController> OnWeaponChanged;

    public static void RaiseWeaponChanged(GunController gun)
    {
        OnWeaponChanged?.Invoke(gun);
    }
}