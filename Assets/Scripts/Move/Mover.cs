using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] protected float Speed = 5f;
    [SerializeField] private float _speedRatio = 1f;
    [SerializeField] private float _jumpForce = 10f;

    protected Rigidbody2D Rigidbody;
    protected Vector2 Velocity = Vector2.zero;

    public event Action<float> Rotate;

    public bool CanMove { get; private set; } = true;
    public bool IsFacingRight { get; protected set; }
    private float VelocityX => _speedRatio * Speed;
    private float VelocityY => Rigidbody.velocity.y;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ChangeDirection()
    {
        int directionChanger = -1;

        Speed *= directionChanger;

        Flip();
    }

    public void Jump(float ratio = 1f)
    {
        Rigidbody.velocity = _jumpForce * ratio * Vector2.up;
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

        if ((_speedRatio < 0) == IsFacingRight && _speedRatio != 0f)
            Flip();
    }

    public virtual void Move()
    {
        Velocity.x = VelocityX;
        Velocity.y = VelocityY;

        Rigidbody.velocity = Velocity;
    }

    public virtual void PushBack(float force)
    {
        Push(force, IsFacingRight);
    }

    public void DisableMoving()
    {
        CanMove = false;
    }

    public void EnableMoving()
    {
        CanMove = true;
    }

    protected void Push(float force, bool isFacingRight)
    {
        Rigidbody.velocity = Vector2.zero;

        if (isFacingRight)
            Rigidbody.AddForce((Vector2.right + Vector2.up) * force, ForceMode2D.Impulse);
        else
            Rigidbody.AddForce((Vector2.left + Vector2.up) * force, ForceMode2D.Impulse);
    }

    protected void Flip()
    {
        float maxRotateAngle = 180f;

        IsFacingRight = !IsFacingRight;

        transform.Rotate(0f, maxRotateAngle, 0f);

        Rotate?.Invoke(maxRotateAngle);
    }
}
