using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;
    private bool _isFacingRight;
    
    private float _speedRatio = 1f;
    private float _jumpForce = 10f;

    public event Action<float> Rotate;

    public bool IsFacingRight => _isFacingRight;
    public float VelocityX => _speedRatio * _speed;
    public float VelocityY => _rigidbody.velocity.y;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ChangeDirection()
    {
        int directionChanger = -1;

        _speed *= directionChanger;

        Flip();
    }

    public void Jump(float ratio = 1f)
    {
        _rigidbody.velocity = _jumpForce * ratio * Vector2.up;
    }

    public void JumpLow()
    {
        float ratio = 0.7f;

        Jump(ratio);
    }

    public void SetMovement(float speedRatio)
    {
        float minRatio = -1f;
        float maxRatio = 1f;

        _speedRatio = Mathf.Clamp(speedRatio, minRatio, maxRatio);

        if ((_speedRatio < 0) == _isFacingRight && _speedRatio != 0f)
            Flip();
    }

    public void Move()
    {
        _velocity.x = VelocityX;
        _velocity.y = VelocityY;

        _rigidbody.velocity = _velocity;
    }

    public void Push(float force, bool isFacingRight)
    {
        _rigidbody.velocity = Vector2.zero;

        if (isFacingRight)
            _rigidbody.AddForce((Vector2.right + Vector2.up) * force, ForceMode2D.Impulse);
        else
            _rigidbody.AddForce((Vector2.left + Vector2.up) * force, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        float maxRotateAngle = 180f;

        _isFacingRight = !_isFacingRight;

        transform.Rotate(0f, maxRotateAngle, 0f);

        Rotate?.Invoke(maxRotateAngle);
    }
}