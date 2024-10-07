using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Mover _mover;
    [SerializeField] private Chase _chase;
    [SerializeField] private Transform _mainTarget;
    [SerializeField] private LayerMask _players;
    [SerializeField] private int _damage = 20;

    private readonly int _isDie = Animator.StringToHash(nameof(_isDie));

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Collider2D _newTarget;
    private Coroutine _coroutineOfChangeTarget;
    private bool _canMove = true;
    private float _aggressiveRadius = 5f;
    private float _stunDuration = 0.5f;
    private float _delayBeforeChangeTarget = 3f;
    private WaitForSeconds _waitForStunEnd;
    private WaitForSeconds _waitForChangeTarget;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _waitForStunEnd = new WaitForSeconds(_stunDuration);
        _waitForChangeTarget = new WaitForSeconds(_delayBeforeChangeTarget);
    }

    private void FixedUpdate()
    {
        if (_canMove)
            _chase.PerformMoveActions();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TriggerPoint triggerPoint))
            _chase.HandleActionsAtTriggerPoint(triggerPoint);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy sameEnemy))
            _chase.HandleCollisionWithSame(sameEnemy.transform);

        if (collision.gameObject.TryGetComponent(out Player player))
            player.TakeDamage(_damage, _mover.IsFacingRight);
    }

    private void Update()
    {
        _chase.ControlFlipOnSameLevelWithTarget();

        HandleOfChangeTarget();
    }

    public void TakeDamage(int damage, bool isFacingRight)
    {
        StartCoroutine(PushWhenTakingDamage(isFacingRight));

        _health.TakeDamage(damage);

        if (_health.CurrentValue <= 0)
            StartCoroutine(Die());
    }

    private void HandleOfChangeTarget()
    {
        _newTarget = Physics2D.OverlapCircle(transform.position, _aggressiveRadius, _players);

        if (_newTarget == null && _chase.CurrentTarget == _mainTarget)
            return;

        if (_newTarget != null)
        {
            if (_coroutineOfChangeTarget != null)
            {
                StopCoroutine(_coroutineOfChangeTarget);
                _coroutineOfChangeTarget = null;
            }

            if (_newTarget.transform != _chase.CurrentTarget)
                _chase.ChangeTarget(_newTarget.transform);
        }
        else
        {
            if (_coroutineOfChangeTarget == null)
                _coroutineOfChangeTarget = StartCoroutine(ChangeTargetAfterDelay());
        }
    }

    private IEnumerator PushWhenTakingDamage(bool isFacingRight)
    {
        float pushForce = 5f;

        _canMove = false;
        _mover.Push(pushForce, isFacingRight);

        yield return _waitForStunEnd;

        _canMove = true;
    }

    private IEnumerator Die()
    {
        float delayBeforeDestroy = 0.4f;
        WaitForSeconds waitForDestroy = new(delayBeforeDestroy);

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetTrigger(_isDie);

        yield return waitForDestroy;

        Destroy(gameObject);
    }

    private IEnumerator ChangeTargetAfterDelay()
    {
        yield return _waitForChangeTarget;

        _chase.ChangeTarget(_mainTarget);
    }
}
