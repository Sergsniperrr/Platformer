using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private GroundChecker _groundChecker;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _isDied;
    private float _velocityX;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isDied)
            return;

        _animator.SetBool(PlayerAnimatorData.s_onGround, _groundChecker.IsGrounded);

        _velocityX = Mathf.Abs(_rigidbody.velocity.x);

        if (_velocityX < 1f)
            _velocityX = 0f;

        _animator.SetFloat(PlayerAnimatorData.s_velocityOnX, _velocityX);
        _animator.SetFloat(PlayerAnimatorData.s_velocityOnY, _rigidbody.velocity.y);
    }

    public void Attack()
    {
        float swingDuration = 0.1f;

        StartCoroutine(Shoot(swingDuration));
    }

    public void Die()
    {
        _isDied = true;

        _animator.SetBool(PlayerAnimatorData.s_onGround, true);
        _animator.Play(PlayerAnimatorData.s_death);
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(PlayerAnimatorData.s_isAttacked);
        OnViewTakeDamage();
    }

    public void DisableTakeDamageEffect()
    {
        _animator.SetBool(PlayerAnimatorData.s_isDamaged, false);
    }

    public void OnViewTakeDamage()
    {
        _animator.SetBool(PlayerAnimatorData.s_isDamaged, true);
    }

    private IEnumerator Shoot(float swingDuration)
    {
        WaitForSeconds swingTime = new WaitForSeconds(swingDuration);

        _animator.SetBool(PlayerAnimatorData.s_isAttack, true);

        yield return swingTime;

        _animator.SetBool(PlayerAnimatorData.s_isAttack, false);
    }
}
