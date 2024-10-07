using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Score _score;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _damage = 20;

    private Rigidbody2D _rigidbody;
    private PlayerInput _input;
    private Coroutine _viewDamage;
    private bool _isJump;
    private bool _canMove = true;
    private float _groundRadius = 0.3f;
    private float _direction;

    public event Action Attack;

    public Mover Mover { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsDamaged { get; private set; }
    public bool IsDied { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Mover = new Mover(transform.transform, _rigidbody, _moveSpeed);
        _input = new();
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;

        IsGrounded = Physics2D.OverlapCircle(_groundChecker.position, _groundRadius, _ground);

        Move(_direction);

        if (_isJump)
        {
            Mover.Jump();
            _isJump = false;
        }
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
            _health.TakeHealing(healthPoint.PowerOfRegeneration);

            healthPoint.PickUp();
        }
    }

    private void Update()
    {
        _direction = _input.VelocityOnX;

        if (IsGrounded && _input.IsJumpKeyPress)
            _isJump = true;

        if (_input.IsShootKeyPress && _bullet.IsEnabled == false)
            Shoot();
    }

    public void TakeDamage(int damage, bool isFacingRight)
    {
        float force = 5;
        float lockDuration = 0.8f;
        WaitForSeconds _timeBeforeControlOn = new(lockDuration);

        if (_health.CurrentValue <= 0)
            return;

        _health.TakeDamage(damage);

        Mover.Push(force, isFacingRight);

        if (_viewDamage != null)
            StopCoroutine(LockControlWhenAttacked(_timeBeforeControlOn));
        
        StartCoroutine(LockControlWhenAttacked(_timeBeforeControlOn));

        if (_health.CurrentValue <= 0)
        {
            float delayBeforeDestroy = 0.8f;
            WaitForSeconds waitingForDestroy = new(delayBeforeDestroy);

            IsDied = true;
            StartCoroutine(Die(waitingForDestroy));
        }
    }

    private void Move(float moveRatio)
    {
        Mover.SetMovement(moveRatio);
        Mover.Move();
    }

    private void Shoot()
    {
        _bullet.Create(_bulletStartPosition.position, Mover.IsFacingRight, _damage);
        Attack();
    }

    private IEnumerator LockControlWhenAttacked(WaitForSeconds timeBeforeControlOn)
    {        
        _canMove = false;
        IsDamaged = true;

        yield return timeBeforeControlOn;

        _canMove = true;
        IsDamaged = false;
    }

    private IEnumerator Die(WaitForSeconds wainingForDestroy)
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;

        yield return wainingForDestroy;

        Destroy(gameObject);
    }
}
