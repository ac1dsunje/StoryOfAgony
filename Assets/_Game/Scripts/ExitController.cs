using System;
using UnityEngine;

namespace _Game.Scripts
{
public class ExitController: MonoBehaviour
{
    [SerializeField] private AnimationClip _creatingAnimation;

    [SerializeField] private Animator _animator;
    
    public event Action OnExit;
    
    private void Awake()
    {
        _animator.Play(_creatingAnimation.name);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        
        player.Clear();
        OnExit?.Invoke();
        
        gameObject.SetActive(false);
    }
}
}