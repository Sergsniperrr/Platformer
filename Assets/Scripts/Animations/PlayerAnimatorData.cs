using UnityEngine;

public static class PlayerAnimatorData
{
    public static readonly int s_velocityOnX = Animator.StringToHash(nameof(s_velocityOnX));
    public static readonly int s_velocityOnY = Animator.StringToHash(nameof(s_velocityOnY));
    public static readonly int s_onGround = Animator.StringToHash(nameof(s_onGround));
    public static readonly int s_isAttack = Animator.StringToHash(nameof(s_isAttack));
    public static readonly int s_isAttacked = Animator.StringToHash(nameof(s_isAttacked));
    public static readonly int s_isDamaged = Animator.StringToHash(nameof(s_isDamaged));
    public static readonly int s_death = Animator.StringToHash(nameof(s_death));
    public static readonly int s_attacked = Animator.StringToHash(nameof(s_attacked));
}
