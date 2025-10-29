using UnityEngine;
using System.Collections;

public class WallPiece : MonoBehaviour
{
    private Rigidbody _rb;
    private string _poolKey = "WallPiece";

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        StartCoroutine(ReturnToPoolAfterDelay(10f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MultiObjectPool.Instance.ReturnToPool(_poolKey, gameObject);
    }
}
