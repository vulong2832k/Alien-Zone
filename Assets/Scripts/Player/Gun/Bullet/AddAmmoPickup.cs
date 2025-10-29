using UnityEngine;

public class AddAmmoPickup : MonoBehaviour
{
    [SerializeField] private AmmoSO ammoData;
    [SerializeField] private float rotateSpeed = 90f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerGuns = other.GetComponentInParent<PlayerController>();
            if (playerGuns != null)
            {
                playerGuns.AddAmmo(ammoData);
            }

            Destroy(gameObject);
        }
    }
}
