using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AuraRotateBlocker : RotateBlocker
{
    public readonly int _vampiricAuraLeft = Animator.StringToHash(nameof(_vampiricAuraLeft));
    public readonly int _vampiricAuraRight = Animator.StringToHash(nameof(_vampiricAuraRight));

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void FlipBack(float angle)
    {
        if (Mover.IsFacingRight)
            _animator.Play(_vampiricAuraRight);
        else
            _animator.Play(_vampiricAuraLeft);
    }
}
