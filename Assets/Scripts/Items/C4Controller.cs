using UnityEngine;

public class C4Controller : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float _fuseTime = 3f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 10f;

    private bool _isPlanted = false;

    public void Plant()
    {
        if (_isPlanted) return;
        _isPlanted = true;
        Invoke(nameof(Explode), _fuseTime);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var col in colliders)
        {
            BreakableWall wall = col.GetComponent<BreakableWall>();
            if (wall != null)
            {
                wall.TriggerByC4();
            }

            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 0.2f, ForceMode.Impulse);
            }
        }

        // TODO: Spawn effect nổ, particle, sound ở đây

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
