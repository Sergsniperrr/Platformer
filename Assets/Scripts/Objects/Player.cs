using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private Score _score;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private int _damage = 20;

    private PlayerInput _input;
    private Coroutine _coroutineOfViewDamage;
    private bool _isInvulnerable;

    public bool IsDamaged { get; private set; }

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Item item))
            item.PickUp();

        if (collision.gameObject.TryGetComponent(out Coin _))
            _score.IncreaseNumberOfCoin();

        if (collision.gameObject.TryGetComponent(out HealthPoint healthPoint))
            _health.TakeHealing(healthPoint.PowerOfRegeneration);
    }

    private void Update()
    {
        if (_input.IsShootKeyPress && _bullet.IsEnabled == false)
            Shoot();
    }

    public void TakeDamage(int damage)
    {
        float pushForce = 6f;
        float lockDuration = 0.8f;
        WaitForSeconds _timeBeforeControlOn = new(lockDuration);

        if (_isInvulnerable)
            return;

        if (_health.CurrentValue <= 0)
            return;

        _health.TakeDamage(damage);

        if (_coroutineOfViewDamage != null)
            StopCoroutine(_coroutineOfViewDamage);

        if (_health.CurrentValue > 0)
        {
            _animator.TakeDamage();
            _mover.PushBack(pushForce);
            _coroutineOfViewDamage = StartCoroutine(LockControlWhenAttacked(_timeBeforeControlOn));
        }
        else
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        float delayBeforeDestroy = 0.8f;
        WaitForSeconds waitingForDestroy = new(delayBeforeDestroy);

        _animator.Die();

        StartCoroutine(Die(waitingForDestroy));
    }

    private void Shoot()
    {
        _bullet.Create(_bulletStartPosition.position, _mover.IsFacingRight, _damage);
        _animator.Attack();
    }

    private IEnumerator LockControlWhenAttacked(WaitForSeconds timeBeforeControlOn)
    {
        _isInvulnerable = true;
        IsDamaged = true;
        _mover.DisableMoving();

        yield return timeBeforeControlOn;

        _isInvulnerable = false;
        _mover.EnableMoving();
        IsDamaged = false;
        _animator.DisableTakeDamageEffect();
    }

    private IEnumerator Die(WaitForSeconds wainingForDestroy)
    {
        _mover.Die();

        yield return wainingForDestroy;

        Destroy(gameObject);
    }
}
