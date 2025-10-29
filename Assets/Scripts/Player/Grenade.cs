using System;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GrenadeSO _data;
    [SerializeField] private string _explosionKey = "EffectExplosionMissile";

    private Rigidbody _rb;
    private float _timer;
    private bool _isActive;
    public void SetData(GrenadeSO data)
    {
        _data = data;
    }
    private void OnEnable()
    {
        _isActive = false;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Activate(Vector3 force, float fuse = -1f)
    {
        if (_data == null) return;
        _isActive = true;
        _timer = fuse > 0f ? fuse : 5f;

        if (_rb != null)
        {
            _rb.isKinematic = false;

            _rb.AddForce(force, ForceMode.Impulse);
        }
    }
    private void Update()
    {
        if (!_isActive) return;
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Explode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_isActive) return;
        Explode();
    }

    private void Explode()
    {
        _isActive = false;

        if (!string.IsNullOrEmpty(_explosionKey))
        {
            MultiObjectPool.Instance.SpawnFromPool(_explosionKey, transform.position, Quaternion.identity);
        }
        Collider[] hits = Physics.OverlapSphere(transform.position, _data.radius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable target) || hit.transform.root.TryGetComponent(out target))
            {
                target.TakeDamage(_data.damage);
            }
        }

        MultiObjectPool.Instance.ReturnToPool("Grenade", gameObject);
    }
}
