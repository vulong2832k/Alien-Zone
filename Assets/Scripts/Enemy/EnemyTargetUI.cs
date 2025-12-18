using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTargetUI : MonoBehaviour
{
    public static EnemyTargetUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Vector3 _worldOffset = new Vector3(0, 2.2f, 0);

    private EnemyController _target;
    private Camera _camera;

    private void Awake()
    {
        Instance = this;
        _camera = Camera.main;
        _panel.SetActive(false);
    }
    private void Update()
    {
        if (_target == null) return;

        Vector3 screenPos = _camera.WorldToScreenPoint(
            _target.transform.position + _worldOffset
        );

        _panel.transform.position = screenPos;
        UpdateHP();
    }
    public void Show(EnemyController enemy)
    {
        this._target = enemy;
        _panel.SetActive(true);

        _nameText.text = enemy.dataEnemy.Name;
        UpdateHP();
    }
    public void Hide()
    {
        _target = null;
        _panel.SetActive(false);
    }
    private void UpdateHP()
    {
        float percent = (float)_target.CurrentHP / _target.MaxHP;
        _hpBar.fillAmount = percent;
    }
}
