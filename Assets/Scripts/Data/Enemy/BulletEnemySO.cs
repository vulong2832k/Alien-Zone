using UnityEngine;

[CreateAssetMenu(fileName = "BulletEnemySO", menuName = "DataBullet")]
public class BulletEnemySO : ScriptableObject
{
    [Header("Basic Settings")]
    public string poolKey;
    public float speed;
    public float lifeTime = 5f;

    [Header("Special Effects")]
    public bool explosive = false;
    public float explosionRadius;
    public int explosionDamage;
}
