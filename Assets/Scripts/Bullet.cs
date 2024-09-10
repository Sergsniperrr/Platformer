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
    private int _damage;
    private bool _isFacingRight;

    public bool IsEnabled { get; private set; }

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

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
            collision.gameObject.GetComponent<Enemy>().TakeDamage(_damage, _isFacingRight);

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        float explosionDuration = 0.15f;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.isKinematic = true;
        _animator.SetBool(_isEnabled, false);

        yield return new WaitForSeconds(explosionDuration);

        _renderer.enabled = false;
        _collider.enabled = false;

        IsEnabled = false;
    }
}
