using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private int _damage = 20;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            player.TakeDamage(_damage);
    }

    public void TakeDamage(float damage)
    {
        float pushForce = 5f;

        _mover.PushBack(pushForce);

        _health.Decrease(damage);

        if (_health.IsZeroValue)
            StartCoroutine(Die());
    }

    public float TransferHealth(float health)
    {
        if (_health.CurrentValue == 0)
            return 0;

        health = Mathf.Min(health, _health.CurrentValue);

        _health.DecreaseFast(health);

        if (_health.IsZeroValue)
            StartCoroutine(Die());

        return health;
    }

    private IEnumerator Die()
    {
        float delayBeforeDestroy = 0.4f;
        WaitForSeconds waitForDestroy = new(delayBeforeDestroy);

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.Die();

        yield return waitForDestroy;

        Destroy(gameObject);
    }
}
