using UnityEngine;

public class NormalShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, string bulletKey)
    {
        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(
            bulletKey,
            firePoint.position,
            firePoint.rotation
        );

        if (bullet != null)
        {
            PlayerBulletNormal bulletScript = bullet.GetComponent<PlayerBulletNormal>();
            if (bulletScript != null)
                bulletScript.SetDamage(gunData.Damage);
        }
    }
}
