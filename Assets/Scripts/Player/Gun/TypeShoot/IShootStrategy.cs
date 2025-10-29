using UnityEngine;

public interface IShootStrategy
{
    void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, string bulletKey);
}
