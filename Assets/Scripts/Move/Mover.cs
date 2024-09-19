using UnityEngine;

public class Mover
{
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Vector2 _velocity = Vector2.zero;
    private bool _isFacingRight;
    private float _speed;
    private float _speedRatio = 1f;
    private float _jumpForce = 10f;

    public bool IsFacingRight => _isFacingRight;

    public Mover(Transform transform, Rigidbody2D rigidbody, float speed)
    {
        _transform = transform;
        _rigidbody = rigidbody;
        _speed = speed;
    }

    public float VelocityX => _speedRatio * _speed;

    public float VelocityY => _rigidbody.velocity.y;

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

        _transform.Rotate(0f, maxRotateAngle, 0f);
    }
}