using UnityEngine;
using TMPro;

public class HUDQuestComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text _questText;
    [SerializeField] private GameObject _conditionsObject;

    private IWinCondition _condition;

    private void Start()
    {
        _condition = _conditionsObject.GetComponent<IWinCondition>();
    }

    private void Update()
    {
        if (_condition != null)
            _questText.text = _condition.GetDescription();
    }
}
