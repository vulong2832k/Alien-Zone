using UnityEngine;

public class NormalShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, string bulletKey)
    {
        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(bulletKey, firePoint.position, firePoint.rotation);

        if (bullet != null)
        {
            PlayerBulletBase bulletScript = bullet.GetComponent<PlayerBulletBase>();
            if (bulletScript != null)
            {
                bulletScript.SetPoolKey(bulletKey);
                bulletScript.SetDamage(gunData.Damage);
            }
        }
    }
}
