using UnityEngine;


public class PlayerMover : Mover
{
    [SerializeField] private GroundChecker _groundChecker;

    private PlayerInput _input = new();
    private bool _isJump = false;
    private float _direction;

    private void FixedUpdate()
    {
        if (CanMove == false)
            return;

        Move();

        if (_isJump)
        {
            Jump();
            _isJump = false;
        }
    }

    private void Update()
    {
        _direction = _input.VelocityOnX;

        if (_groundChecker.IsGrounded && _input.IsJumpKeyPress)
            _isJump = true;
    }

    public override void Move()
    {
        SetMovement(_direction);

        base.Move();
    }

    public void Die()
    {
        Rigidbody.velocity = Vector2.zero;

        DisableMoving();

        Rigidbody.isKinematic = true;
    }
}