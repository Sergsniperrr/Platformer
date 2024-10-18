using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private readonly int _isDie = Animator.StringToHash(nameof(_isDie));

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Die()
    {
        _animator.SetTrigger(_isDie);
    }
}
