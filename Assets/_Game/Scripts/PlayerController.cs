using System;
using System.Collections.Generic;
using _Game.Scripts.Fire;
using _Game.Scripts.Items;
using _Game.Scripts.Items.Box;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
public class PlayerController : MonoBehaviour, IDamageAble
{
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;
    
    private readonly List<QuestItem> _objects = new();
    
    private readonly List<BoxItem> _boxes = new();

    private BoxItem _nearestBox;

    public event Action<List<QuestItem>> OnItemsChanged;

    private Tween _scaleTween;
    private Vector3 _defaultScale;
    
    private void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _defaultScale = transform.localScale;
    }

    private void Update()
    {
        ReadInput();
        UpdateMovementAnimation();
        _nearestBox = GetNearestBox();
    }

    private void ReadInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        TryUnPackBoxes();
    }

    private void UpdateMovementAnimation()
    {
        bool isMoving = Mathf.Abs(_horizontalInput) > 0.1f || Mathf.Abs(_verticalInput) > 0.1f;

        if (isMoving)
        {
            if (_scaleTween == null || !_scaleTween.IsActive())
            {
                _scaleTween = transform.DOScale(new Vector3(1.3f, 0.7f, 1f), 0.15f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            }
        }
        else
        {
            if (_scaleTween != null && _scaleTween.IsActive())
            {
                _scaleTween.Kill();
                _scaleTween = null;
                
                transform.DOScale(_defaultScale, 0.25f).SetEase(Ease.OutBack);
            }
        }
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
            _objects.Add(item);
            OnItemsChanged?.Invoke(_objects);
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
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _verticalInput * _speed);

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput > 0;
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
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        _objects.Clear();
        OnItemsChanged?.Invoke(_objects);
    }
    
    private void OnDestroy()
    {
        if (_scaleTween != null && _scaleTween.IsActive())
        {
            _scaleTween.Kill();
        }
    }
}
}