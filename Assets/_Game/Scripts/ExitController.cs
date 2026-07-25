using System;
using UnityEngine;

namespace _Game.Scripts
{
public class ExitController: MonoBehaviour
{
    private Animator _animator;
    public event Action OnExit;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Creating");
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