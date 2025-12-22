using UnityEngine;
using DG.Tweening;

public enum ScreenEffectType
{
    Hurt,
    Heal,
    Death
}
public abstract class ScreenEffectBase : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup.alpha = 0f;
    }

    public virtual void Play(float duration = 0.2f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = 1f;
        _canvasGroup.DOFade(0f, duration);
    }
}
