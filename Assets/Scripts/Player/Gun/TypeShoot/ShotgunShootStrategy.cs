using UnityEngine;

public class ShotgunShootStrategy : IShootStrategy
{
    public void Shoot(Transform firePoint, Vector3 shootDirection, GunAttributes gunData, string bulletKey)
    {
        int quantityPerShoot = gunData.QuantityPerShoot;
        float spearAngle = gunData.SpeardAngle;

        for (int i = 0; i < quantityPerShoot; i++)
        {
            Quaternion spreadRotation = Quaternion.Euler(
                Random.Range(-spearAngle, spearAngle),
                Random.Range(-spearAngle, spearAngle),
                0
            );

            Vector3 pelletDir = spreadRotation * shootDirection;

            GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(
                bulletKey,
                firePoint.position,
                Quaternion.LookRotation(pelletDir)
            );

            if (bullet != null)
            {
                PlayerBulletNormal bulletPlayer = bullet.GetComponent<PlayerBulletNormal>();
                if (bulletPlayer != null)
                    bulletPlayer.SetDamage(gunData.Damage / quantityPerShoot);
            }
        }
    }
}
