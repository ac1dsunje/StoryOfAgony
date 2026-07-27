using System;
using System.Collections.Generic;
using _Game.Scripts.Fire;
using _Game.Scripts.Items;
using _Game.Scripts.Items.Box;
using UnityEngine;

namespace _Game.Scripts.Player
{
public class PlayerController : MonoBehaviour, IDamageAble
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;
    
    private readonly List<BoxItem> _boxes = new();
    private readonly List<QuestItem> _items = new();

    private BoxItem _nearestBox;
    private bool _isMoving;

    public event Action<List<QuestItem>> OnItemsChanged;
    public event Action OnDeath;
    public event Action<bool> OnMovingChanged;
    
    private void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
        _nearestBox = GetNearestBox();
    }

    private void ReadInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        TryUnPackBoxes();
    }

    private void TryUnPackBoxes()
    {
        if (!Input.GetKeyDown(KeyCode.E) || _boxes.Count == 0)
            return;

        if (_nearestBox == null) return;
        var items = _nearestBox.GetItems();
        _nearestBox.TakeHit();

        foreach (var item in items)
        {
            _items.Add(item);
            OnItemsChanged?.Invoke(_items);
        }

        _boxes.Remove(_nearestBox);  
    }

    private BoxItem GetNearestBox()
    {
        BoxItem nearestBox = null;
        var nearestDistance = float.MaxValue;

        foreach (var box in _boxes)
        {
            var distance = Vector3.SqrMagnitude(
                box.transform.position - transform.position);

            if (!(distance < nearestDistance)) continue;
            nearestDistance = distance;
            nearestBox = box;
        }

        nearestBox?.SetLighter(true);

        if (nearestBox != _nearestBox)
        {
            _nearestBox?.SetLighter(false);
        }
        
        return nearestBox;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _verticalInput * _speed);

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput > 0;
        }

        var isMoving = Mathf.Abs(_rb.linearVelocity.x) > 0.1f || Mathf.Abs(_rb.linearVelocity.y) > 0.1f;
        
        if (isMoving != _isMoving)
        {
            _isMoving = isMoving;
            OnMovingChanged?.Invoke(_isMoving);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<BoxItem>(out var boxItem)) return;
        if (_boxes.Contains(boxItem)) return;
        _boxes.Add(boxItem);
    }

    public void TakeHit()
    {
        OnDeath?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<BoxItem>(out var boxItem)) return;
        if (boxItem.IsEmpty) return;
        boxItem.SetLighter(false);
        _boxes.Remove(boxItem);
    }

    public void Clear()
    {
        _items.Clear();
        OnItemsChanged?.Invoke(_items);
    }
}
}