using UnityEngine;

public class PlayerCameraThirdPerson : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -3.5f);
    [SerializeField] private float sensitivity = 200f;
    [SerializeField] private float minYAngle = -30f;
    [SerializeField] private float maxYAngle = 70f;
    [SerializeField] private float smoothSpeed = 10f;

    private float yaw;
    private float pitch;

    void Start()
    {
        if (_player == null)
            Debug.LogWarning("Camera chưa gán Player!");

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (!_player) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = _player.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(_player.position + Vector3.up * 1.5f);

        _player.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
