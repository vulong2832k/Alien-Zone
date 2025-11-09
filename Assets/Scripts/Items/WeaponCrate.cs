using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    private Animator _animator;

    [Header("Mission Crate Settings")]
    [SerializeField] private bool _isMissionCrate = false;
    [SerializeField] private MonoBehaviour _winConditionRef;

    private IWinCondition _winCondition;
    private bool _opened = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
         
        if (_isMissionCrate && _winConditionRef != null)
        {
            _winCondition = _winConditionRef as IWinCondition;
        }
        else if (_isMissionCrate)
        {
            _winCondition = FindFirstObjectByType<FindSecretsCondition>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E))
        {
            _animator.SetBool("Open", true);

            if (!_opened)
            {
                _opened = true;

                if (_isMissionCrate && _winCondition != null)
                {
                    if (_winCondition is FindSecretsCondition secretCondition)
                    {
                        secretCondition.RegisterSecretFound();
                        Debug.Log("Mission secret registered!");
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("Open", false);
    }
}
