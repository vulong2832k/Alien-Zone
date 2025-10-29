using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform _doorObject;
    [SerializeField] private float _openScaleY = 0f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _autoCloseDelay = 5f;

    private Vector3 _originalScale;
    private bool _isNearPlayer = false;
    private bool _isOpen = false;

    private void Start()
    {
        _originalScale = _doorObject.localScale;
    }
    private void Update()
    {
        if (_isNearPlayer && Input.GetKeyDown(KeyCode.E) && !_isOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpen = true;

        _doorObject.DOScaleY(_openScaleY, _duration).SetEase(Ease.InOutSine);
        DOVirtual.DelayedCall(_autoCloseDelay, () => CloseDoor());
    }

    private void CloseDoor()
    {
        _doorObject.DOScaleY(_originalScale.y, _duration).SetEase(Ease.InOutSine).OnComplete(() => _isOpen = false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isNearPlayer = false;
        }
    }
}
