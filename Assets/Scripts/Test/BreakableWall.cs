using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public enum WallType { BulletBreakable, C4Breakable }

    [Header("Wall Settings")]
    [SerializeField] private WallType _wallType = WallType.BulletBreakable;
    [SerializeField] private int _hitsToBreak;

    [Header("Explosion Settings")]
    [SerializeField] private int _pieces = 3;
    [SerializeField] private float _explosionForce = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private string _pieceKey = "WallPiece";

    private int _currentHits;

    public void TakeDamage(int damage)
    {
        if (_wallType != WallType.BulletBreakable) return;
        if (damage <= 0) return;

        _currentHits += damage;

        if (_currentHits >= _hitsToBreak)
        {
            Explode();
        }
    }

    public void TriggerByC4()
    {
        if (_wallType != WallType.C4Breakable) return;
        Explode();
    }

    private void Explode()
    {
        Vector3 wallCenter = GetComponent<Renderer>().bounds.center;
        Vector3 pieceSize = transform.localScale / _pieces;

        for (int x = 0; x < _pieces; x++)
        {
            for (int y = 0; y < _pieces; y++)
            {
                for (int z = 0; z < _pieces; z++)
                {
                    GameObject piece = MultiObjectPool.Instance.SpawnFromPool(_pieceKey, transform.position, transform.rotation);
                    if (piece == null) continue;

                    piece.transform.localScale = pieceSize;

                    Vector3 offset = new Vector3(
                        (x - (_pieces - 1) / 2f) * pieceSize.x,
                        (y - (_pieces - 1) / 2f) * pieceSize.y,
                        (z - (_pieces - 1) / 2f) * pieceSize.z
                    );

                    piece.transform.position = wallCenter + offset;

                    Rigidbody rb = piece.GetComponent<Rigidbody>();
                    if (rb == null) rb = piece.AddComponent<Rigidbody>();

                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    rb.AddExplosionForce(_explosionForce, wallCenter, _explosionRadius, 0.1f, ForceMode.Impulse);
                }
            }
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);

        int arrowCount = 12;
        for (int i = 0; i < arrowCount; i++)
        {
            float angle = (360f / arrowCount) * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, dir * (_explosionForce * 0.1f));
        }
    }
}
