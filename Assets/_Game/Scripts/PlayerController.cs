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
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _verticalInput * _speed);

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput > 0;
        }
    }
}
}
