using DG.Tweening;
using UnityEngine;

public class HealScreen : ScreenEffectBase
{
    [SerializeField] private float _maxAlpha = 0.4f;

    public override void Play(float duration = 0.25f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = 0f;

        _canvasGroup
            .DOFade(this._maxAlpha, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                _canvasGroup.DOFade(0f, duration);
            });
    }
}
