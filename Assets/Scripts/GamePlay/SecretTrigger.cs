using UnityEngine;

public class SecretTrigger : MonoBehaviour
{
    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        FindSecretsCondition condition = Object.FindAnyObjectByType<FindSecretsCondition>();
        if (condition != null)
        {
            condition.RegisterSecretFound();
            _triggered = true;
            gameObject.SetActive(false);
        }
    }
}
