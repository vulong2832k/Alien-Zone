using DG.Tweening;
using UnityEngine;

public class HurtScreen : ScreenEffectBase
{
    [SerializeField] private float _maxAlpha = 0.6f;

    public override void Play(float duration = 0.15f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = this._maxAlpha;
        _canvasGroup.DOFade(0f, duration).SetEase(Ease.OutQuad);
    }
}
