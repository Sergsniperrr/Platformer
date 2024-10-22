using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private int _damage = 20;

    private PlayerInput _input;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void Update()
    {
        if (_input.IsShootKeyPress && _bullet.IsEnabled == false)
            Shoot();
    }

    private void Shoot()
    {
        _bullet.Create(_bulletStartPosition.position, _mover.IsFacingRight, _damage);
        _animator.Attack();
    }
}
