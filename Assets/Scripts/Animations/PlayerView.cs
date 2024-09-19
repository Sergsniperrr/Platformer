using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Player _player;

    private readonly int _velocityOnX = Animator.StringToHash(nameof(_velocityOnX));
    private readonly int _velocityOnY = Animator.StringToHash(nameof(_velocityOnY));
    private readonly int _onGround = Animator.StringToHash(nameof(_onGround));
    private readonly int _isAttack = Animator.StringToHash(nameof(_isAttack));
    private readonly int _isAttacked = Animator.StringToHash(nameof(_isAttacked));
    private readonly int _isDamaged = Animator.StringToHash(nameof(_isDamaged));

    private Animator _animator;
    private bool _isTakenDamage;

    private void Awake()
    {
        _animator = _player.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _player.Attack += Attack;
    }

    private void OnDisable()
    {
        _player.Attack -= Attack;
    }

    private void Update()
    {
        _animator.SetBool(_onGround, _player.IsGrounded);

        _animator.SetFloat(_velocityOnX, Mathf.Abs(_player.Mover.VelocityX));
        _animator.SetFloat(_velocityOnY, _player.Mover.VelocityY);

        if (_isTakenDamage != _player.IsDamaged)
            ViewTakeDamage(_player.IsDamaged);

        if (_player.IsDied)
            Die();
    }

    private void Attack()
    {
        float swingDuration = 0.1f;

        StartCoroutine(Shoot(swingDuration));
    }

    private IEnumerator Shoot(float swingDuration)
    {
        _animator.SetBool(_isAttack, true);

        yield return new WaitForSeconds(swingDuration);

        _animator.SetBool(_isAttack, false);
    }

    private void ViewTakeDamage(bool isDamaged)
    {
        if (isDamaged)
        {
            _animator.SetTrigger(_isAttacked);
            _animator.SetBool(_isDamaged, true);
        }
        else
            _animator.SetBool(_isDamaged, false);

        _isTakenDamage = _player.IsDamaged;
    }

    private void Die()
    {
        string animationName = "Death";

        _animator.Play(animationName);
    }
}
