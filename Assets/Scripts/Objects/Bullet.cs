using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
public class Bullet : MonoBehaviour
{
    private readonly int _isEnabled = Animator.StringToHash(nameof(_isEnabled));

    private SpriteRenderer _renderer;
    private CircleCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private float _force = 10f;
    private float _explosionDuration = 0.15f;
    private WaitForSeconds _waitEndOfExplosion;
    private int _damage;
    private bool _isFacingRight;

    public bool IsEnabled { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _waitEndOfExplosion = new(_explosionDuration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
            enemy.TakeDamage(_damage);

        _collider.enabled = false;

        StartCoroutine(Destroy());
    }

    public void Create(Vector2 startPosition, bool isFacingRight, int damage)
    {
        _damage = damage;
        transform.position = startPosition;
        _isFacingRight = isFacingRight;

        _renderer.enabled = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;

        if (_isFacingRight)
            _rigidbody.velocity = Vector2.right * _force;
        else
            _rigidbody.velocity = Vector2.left * _force;

        IsEnabled = true;

        _animator.SetBool(_isEnabled, IsEnabled);
    }

    private IEnumerator Destroy()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetBool(_isEnabled, false);

        yield return _waitEndOfExplosion;

        _renderer.enabled = false;
        IsEnabled = false;
    }
}
