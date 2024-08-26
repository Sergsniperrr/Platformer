using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);

    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private CoinSpawner _coins;
    [SerializeField] private float _moveSpeed = 5f;

    private readonly int _velocityOnX = Animator.StringToHash(nameof(_velocityOnX));
    private readonly int _velocityOnY = Animator.StringToHash(nameof(_velocityOnY));
    private readonly int _onGround = Animator.StringToHash(nameof(_onGround));

    private Mover _mover;
    private Animator _animator;
    private bool _isGrounded;
    private float _groundRadius = 0.3f;
    private float _direction;
    private bool _isJump;

    private void Start()
    {
        _mover = new Mover(transform.transform, GetComponent<Rigidbody2D>(), _moveSpeed);
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundRadius, _ground);

        Move(_direction);

        if (_isJump)
        {
            _mover.Jump();
            _isJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Coin coin))
        {
            _coins.PickUpCoin();
            Destroy(coin.gameObject);
        }
    }

    private void Update()
    {
        _animator.SetBool(_onGround, _isGrounded);

        _direction = Input.GetAxis(Horizontal);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
            _isJump = true;
    }

    private void Move(float moveRatio)
    {
        _mover.SetMovement(moveRatio);
        _mover.Move();

        _animator.SetFloat(_velocityOnX, Mathf.Abs(_mover.VelocityX));
        _animator.SetFloat(_velocityOnY, _mover.VelocityY);
    }
}
