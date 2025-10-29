using UnityEngine;
using System.Collections;

public class PoolEffect : MonoBehaviour
{
    [SerializeField] private string poolKey = "HitEffect";
    [SerializeField] private float lifeTime = 1.5f;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToPoolAfterDelay(lifeTime));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        MultiObjectPool.Instance.ReturnToPool(poolKey, gameObject);
    }
}
