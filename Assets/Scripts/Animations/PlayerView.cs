using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Player _player;

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
        _animator.SetBool(PlayerAnimatorData._onGround, _player.IsGrounded);

        _animator.SetFloat(PlayerAnimatorData._velocityOnX, Mathf.Abs(_player.Mover.VelocityX));
        _animator.SetFloat(PlayerAnimatorData._velocityOnY, _player.Mover.VelocityY);

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
        _animator.SetBool(PlayerAnimatorData._isAttack, true);

        yield return new WaitForSeconds(swingDuration);

        _animator.SetBool(PlayerAnimatorData._isAttack, false);
    }

    private void ViewTakeDamage(bool isDamaged)
    {
        if (isDamaged)
        {
            _animator.SetTrigger(PlayerAnimatorData._isAttacked);
            _animator.SetBool(PlayerAnimatorData._isDamaged, true);
        }
        else
        {
            _animator.SetBool(PlayerAnimatorData._isDamaged, false);
        }

        _isTakenDamage = _player.IsDamaged;
    }

    private void Die()
    {
        _animator.Play(PlayerAnimatorData._death);
    }
}
