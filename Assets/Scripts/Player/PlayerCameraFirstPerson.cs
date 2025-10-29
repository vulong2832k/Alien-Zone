using UnityEngine;

public class PlayerCameraFirstPerson : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 180f;
    [SerializeField] private Transform _player;

    private float _xRotation = 0f;

    void Start()
    {
        if (_player == null)
            _player = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoveCamera();
    }
    private void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _player.Rotate(Vector3.up * mouseX);
    }
}
