using System.Collections.Generic;
using _Game.Scripts.Items;
using _Game.Scripts.Items.Box;
using UnityEngine;

namespace _Game.Scripts
{
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;
    
    [SerializeField] private List<QuestItem> _objects = new();
    
    private readonly List<BoxItem> _boxes = new();
    
    private void Awake() 
    {
        _rb  = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        TryUnPackBoxes();
    }

    private void TryUnPackBoxes()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        foreach (var box in _boxes)
        {
            foreach (var item in box.GetItems())
            {
                _objects.Add(item);
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _verticalInput * _speed);

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput > 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<BoxItem>(out var boxItem)) return;
        
        _boxes.Add(boxItem);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<BoxItem>(out var boxItem)) return;
        
        _boxes.Remove(boxItem);
    }
}
}
