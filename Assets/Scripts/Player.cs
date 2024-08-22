using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : Mover
{
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _ground;

    private readonly int _velocityOnX = Animator.StringToHash(nameof(_velocityOnX));
    private readonly int _velocityOnY = Animator.StringToHash(nameof(_velocityOnY));
    private readonly int _onGround = Animator.StringToHash(nameof(_onGround));

    private Animator _animator;
    private bool _isGrounded;
    private float _groundRadius = 0.3f;
    private float _jumpForce = 10f;
    private float _moveInput;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Speed = 5f;
        Direction = new Vector2(_moveInput * Speed, Rigidbody.velocity.y);
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = SetMoving(Input.GetAxis("Horizontal"));
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundRadius, _ground);

        _animator.SetBool(_onGround, _isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            Rigidbody.velocity = Vector2.up * _jumpForce;
    }

    private Vector2 SetMoving(float moveInput)
    {
        Direction.x = moveInput * Speed;
        Direction.y = Rigidbody.velocity.y;

        if ((moveInput < 0) == IsFacingRight && moveInput != 0f)
            Flip();

        _animator.SetFloat(_velocityOnX, Mathf.Abs(Direction.x));
        _animator.SetFloat(_velocityOnY, Direction.y);

        return Direction;
    }
}
