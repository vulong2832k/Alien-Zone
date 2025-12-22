using DG.Tweening;
using UnityEngine;

public class DeathScreen : ScreenEffectBase
{
    [SerializeField] private float _holdTime = 3f;

    public override void Play(float duration = 1.5f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = 0f;

        _canvasGroup.DOFade(1f, 0.1f)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(this._holdTime, () =>
                {
                    _canvasGroup.DOFade(0f, duration);
                });
            });
    }
}
