using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    [SerializeField] private SFXType _hoverSound = SFXType.ButtonHover;
    [SerializeField] private SFXType _clickSound = SFXType.ButtonClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(_hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(_clickSound);
    }
}
