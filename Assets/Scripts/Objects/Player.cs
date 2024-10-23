using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private Score _score;

    private Coroutine _coroutineOfViewDamage;
    private bool _isInvulnerable;

    public bool IsDamaged { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Item item))
            item.PickUp();

        if (collision.gameObject.TryGetComponent(out Coin _))
            _score.IncreaseNumberOfCoin();

        if (collision.gameObject.TryGetComponent(out HealthPoint healthPoint))
            _health.Increase(healthPoint.PowerOfRegeneration);
    }

    public void TakeDamage(float damage)
    {
        float pushForce = 6f;
        float lockDuration = 0.8f;
        WaitForSeconds _timeBeforeControlOn = new(lockDuration);

        if (_isInvulnerable)
            return;

        if (_health.IsZeroValue)
            return;

        _health.Decrease(damage);

        if (_coroutineOfViewDamage != null)
            StopCoroutine(_coroutineOfViewDamage);

        if (_health.IsZeroValue == false)
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
