using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Player
{
public class PlayerAnimator: IDisposable
{
    private readonly PlayerController _player;
    private readonly Vector3 _defaultScale;

    private Tween _scaleTween;
    
    public PlayerAnimator(PlayerController player)
    {
        _player = player;
        _player.OnMovingChanged += UpdateMovementAnimation;
        _defaultScale = _player.transform.localScale;
    }
    
    private void UpdateMovementAnimation(bool isMoving)
    {
        if (isMoving)
        {
            if (_scaleTween != null && _scaleTween.IsActive()) return;
            _scaleTween = _player.transform.DOScale(new Vector3(1.3f, 0.7f, 1f), 0.15f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else
        {
            if (_scaleTween == null || !_scaleTween.IsActive()) return;
            _scaleTween.Kill();
            _scaleTween = null;
                
            _player.transform.DOScale(_defaultScale, 0.25f).SetEase(Ease.OutBack);
        }
    }

    public void Dispose()
    {
        if (_scaleTween != null && _scaleTween.IsActive())
        {
            _scaleTween.Kill();
        }
        _player.OnMovingChanged -= UpdateMovementAnimation;
    }
}
}