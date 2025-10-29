using System.Collections.Generic;
using UnityEngine;

public static class GunDataManager
{
    private static Dictionary<string, (int currentAmmo, int reserveAmmo)> gunAmmoData = new();

    public static void SaveAmmo(string gunName, int current, int reserve)
    {
        gunAmmoData[gunName] = (current, reserve);
    }

    public static (int currentAmmo, int reserveAmmo) LoadAmmo(string gunName, int defaultCurrent, int defaultReserve)
    {
        if (gunAmmoData.TryGetValue(gunName, out var data))
            return data;

        return (defaultCurrent, defaultReserve);
    }
}

