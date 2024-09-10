using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);

    [SerializeField] private Score _score;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _damage = 20;

    private readonly int _velocityOnX = Animator.StringToHash(nameof(_velocityOnX));
    private readonly int _velocityOnY = Animator.StringToHash(nameof(_velocityOnY));
    private readonly int _onGround = Animator.StringToHash(nameof(_onGround));
    private readonly int _isAttack = Animator.StringToHash(nameof(_isAttack));
    private readonly int _isAttacked = Animator.StringToHash(nameof(_isAttacked));
    private readonly int _isDamaged = Animator.StringToHash(nameof(_isDamaged));
    private readonly int _isAlive = Animator.StringToHash(nameof(_isAlive));
    private readonly int _isDie = Animator.StringToHash(nameof(_isDie));

    private Mover _mover;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private PlayerInput _input;
    private Coroutine _viewDamage;
    private bool _isGrounded;
    private bool _isJump;
    private bool _canMove = true;
    private float _groundRadius = 0.3f;
    private float _direction;
    private int _currentHealth;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mover = new Mover(transform.transform, _rigidbody, _moveSpeed);
        _animator = GetComponent<Animator>();
        _input = new();
        _currentHealth = _maxHealth;
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;

        _isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundRadius, _ground);

        Move(_direction);

        if (_isJump )
            _isJump = _mover.Jump();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Coin coin))
        {
            _score.IncreaseNomberOfCoin();
            coin.PickUp();
        }

        if (collision.gameObject.TryGetComponent(out HealthPoint healthPoint))
        {
            _currentHealth += healthPoint.PowerOfRegeneration;

            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;

            healthPoint.PickUp();
        }
    }

    private void Update()
    {
        _animator.SetBool(_onGround, _isGrounded);

        _direction = _input.VelocityOnX;

        if (_isGrounded && _input.IsJumpKeyPress)
            _isJump = true;

        if (_input.IsShootKeyPress && _bullet.IsEnabled == false)
            StartCoroutine(Shoot());
    }

    public void TakeDamage(int damage, bool isFacingRight)
    {
        float force = 5;

        if (_currentHealth <= 0)
            return;

        _currentHealth -= damage;

        _mover.Push(force, isFacingRight);

        if (_viewDamage != null)
            StopCoroutine(VisualizeDamageEffect());
        
        StartCoroutine(VisualizeDamageEffect());

        if (_currentHealth <= 0)
            StartCoroutine(Die());
    }

    private void Move(float moveRatio)
    {
        _mover.SetMovement(moveRatio);
        _mover.Move();

        _animator.SetFloat(_velocityOnX, Mathf.Abs(_mover.VelocityX));
        _animator.SetFloat(_velocityOnY, _mover.VelocityY);
    }

    private IEnumerator Shoot()
    {
        float swingDuration = 0.1f;
        bool canAttack;

        _bullet.Create(_bulletStartPosition.position, _mover.IsFacingRight, _damage);
        canAttack = true;

        if (canAttack)
            _animator.SetBool(_isAttack, true);

        yield return new WaitForSeconds(swingDuration);

        _animator.SetBool(_isAttack, false);
    }

    private IEnumerator VisualizeDamageEffect()
    {
        float damageEffectDuration = 0.8f;
        
        _canMove = false;
        _animator.SetTrigger(_isAttacked);
        _animator.SetBool(_isDamaged, true);

        yield return new WaitForSeconds(damageEffectDuration);

        _canMove = true;
        _animator.SetBool(_isDamaged, false);
    }

    private IEnumerator Die()
    {
        float delayBeforeDestroy = 1f;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetBool(_isAlive, false);
        _animator.SetTrigger(_isDie);

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);
    }
}
