using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = -3f;
    [SerializeField] private Transform _mainTarget;
    [SerializeField] private LayerMask _players;
    [SerializeField] private int _health = 60;
    [SerializeField] private int _damage = 20;

    private readonly int _isDie = Animator.StringToHash(nameof(_isDie));

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Transform _currentTarget;
    private Collider2D _newTarget;
    private Chase _chase;
    private Mover _mover;
    private Coroutine _coroutineOfChangeTarget;
    private bool _canMove = true;
    private float _aggressiveRadius = 5f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _mover = new Mover(transform, _rigidbody, _moveSpeed);

        _currentTarget = _mainTarget;
        _chase = new Chase(transform, _currentTarget, _mover);
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

        _health -= damage;

        if (_health <= 0)
            StartCoroutine(Die());
    }

    private void HandleOfChangeTarget()
    {
        _newTarget = Physics2D.OverlapCircle(transform.position, _aggressiveRadius, _players);

        if (_newTarget == null && _currentTarget != _mainTarget)
        {
            _currentTarget = _mainTarget;

            if (_coroutineOfChangeTarget != null)
                StopCoroutine(ChangeTargetAfterDelay());

            _coroutineOfChangeTarget = StartCoroutine(ChangeTargetAfterDelay());
        }
        else if (_newTarget != null && _currentTarget != _newTarget?.transform)
        {
            _currentTarget = _newTarget?.transform;
            _chase.ChangeTarget(_currentTarget);
        }
    }

    private IEnumerator PushWhenTakingDamage(bool isFacingRight)
    {
        float disableControlDuration = 0.5f;
        float pushForce = 5f;

        _canMove = false;
        _mover.Push(pushForce, isFacingRight);

        yield return new WaitForSeconds(disableControlDuration);

        _canMove = true;
    }

    private IEnumerator Die()
    {
        float delayBeforeDestroy = 0.4f;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetTrigger(_isDie);

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);
    }

    private IEnumerator ChangeTargetAfterDelay()
    {
        float delay = 5f;

        yield return new WaitForSeconds(delay);

        _chase.ChangeTarget(_mainTarget);
    }
}
