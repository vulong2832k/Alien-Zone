using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImage : MonoBehaviour
{
    [Header("Layouts: ")]
    [SerializeField] private GameObject _weaponIconPrefab;
    [SerializeField] private Transform _iconParent;

    [Header("Colors: ")]
    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _inactiveColor = new Color(1, 1, 1, 0.3f);

    [Header("Sprite mapping:")]
    [SerializeField] private Sprite _pistolSprite;
    [SerializeField] private Sprite _shotgunSprite;
    [SerializeField] private Sprite _rifleSprite;
    [SerializeField] private Sprite _sniperSprite;
    [SerializeField] private Sprite _smgSprite;
    [SerializeField] private Sprite _mgSprite;

    [Header("Animation: ")]
    [SerializeField] private RectTransform _uiPanel;
    [SerializeField] private Vector2 _showPos = new Vector2(-250, 0);
    [SerializeField] private Vector2 _hiddenPos = new Vector2(250, 0);
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _showTime = 2f;

    private List<Image> _weaponImages = new List<Image>();
    [SerializeField]private Dictionary<GunType, Sprite> _spriteTypeGuns;

    private Tween _currenTween;

    [SerializeField] private List<Image> _presetWeaponIcons;

    private void Start()
    {
        _spriteTypeGuns = new Dictionary<GunType, Sprite>
        {
            {GunType.Pistol, _pistolSprite },
            {GunType.SMG, _smgSprite },
            {GunType.MG, _mgSprite },
            {GunType.Rifle, _rifleSprite },
            {GunType.Sniper, _sniperSprite },
            {GunType.Shotgun, _shotgunSprite },
        };

        _uiPanel.anchoredPosition = _hiddenPos;
    }
    public void GenerateIconGunsByType(List<GunType> gunTypes)
    {
        for (int i = 0; i < _presetWeaponIcons.Count; i++)
        {
            _presetWeaponIcons[i].sprite = null;
            _presetWeaponIcons[i].color = _inactiveColor;
        }

        _weaponImages.Clear();

        for (int i = 0; i < gunTypes.Count && i < _presetWeaponIcons.Count; i++)
        {
            GunType type = gunTypes[i];
            Debug.Log("GunType: " + type);

            if (_spriteTypeGuns.TryGetValue(type, out Sprite sprite))
            {
                _presetWeaponIcons[i].sprite = sprite;
            }

            _weaponImages.Add(_presetWeaponIcons[i]);
        }
    }

    public void UpdateImageUI(int currentIndex)
    {
        for (int i = 0; i < _weaponImages.Count; i++)
        {
            _weaponImages[i].color = (i == currentIndex) ? _activeColor : _inactiveColor;
        }
        _currenTween?.Kill();
        _uiPanel.DOAnchorPos(_showPos, _duration);
        _currenTween = DOVirtual.DelayedCall(_showTime, () =>
        {
            _uiPanel.DOAnchorPos(_hiddenPos, _duration);
        });
    }
}
