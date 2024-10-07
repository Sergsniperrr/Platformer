using UnityEngine;

public static class PlayerAnimatorData
{
    public static readonly int _velocityOnX = Animator.StringToHash(nameof(_velocityOnX));
    public static readonly int _velocityOnY = Animator.StringToHash(nameof(_velocityOnY));
    public static readonly int _onGround = Animator.StringToHash(nameof(_onGround));
    public static readonly int _isAttack = Animator.StringToHash(nameof(_isAttack));
    public static readonly int _isAttacked = Animator.StringToHash(nameof(_isAttacked));
    public static readonly int _isDamaged = Animator.StringToHash(nameof(_isDamaged));
    public static readonly int _death = Animator.StringToHash(nameof(_death));
}
