using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ElevatorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _elevator;
    [SerializeField] private Transform _downPosition;

    [Header("Settings")]
    [SerializeField] private float _moveDuration = 3f;
    [SerializeField] private float _rotateDuration = 2f;
    [SerializeField] private KeyCode _interactKey = KeyCode.E;

    private Vector3 _startPos;
    private Quaternion _startRot;
    private bool _isMoving = false;
    private bool _isDown = false;
    private bool _playerInside = false;

    private void Start()
    {
        _startPos = _elevator.position;
        _startRot = _elevator.rotation;
    }

    private void Update()
    {
        if (_playerInside && !_isMoving && Input.GetKeyDown(_interactKey))
        {
            if (_isDown)
                StartCoroutine(MoveElevator(_startPos, _startRot));
            else
                StartCoroutine(MoveElevator(_downPosition.position, Quaternion.Euler(0, 90, 0)));
        }
    }

    private IEnumerator MoveElevator(Vector3 targetPos, Quaternion targetRot)
    {
        _isMoving = true;

        Vector3 startPos = _elevator.position;
        Quaternion startRot = _elevator.rotation;

        float time = 0f;
        while (time < _moveDuration)
        {
            float t = time / _moveDuration;
            _elevator.position = Vector3.Lerp(startPos, targetPos, t);
            _elevator.rotation = Quaternion.Slerp(startRot, targetRot, t);
            time += Time.deltaTime;
            yield return null;
        }

        _elevator.position = targetPos;
        _elevator.rotation = targetRot;

        _isDown = !_isDown;
        _isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInside = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_downPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_downPosition.position, 0.2f);
        }
    }
}
