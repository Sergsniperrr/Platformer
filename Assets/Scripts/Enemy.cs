using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _obstacleRayPoint;
    [SerializeField] private float _moveSpeed = -3f;

    private Mover _mover;
    private RaycastHit2D _hitGround;
    private bool _isHaveTarget = false;

    private void Start()
    {
        _mover = new Mover(transform, GetComponent<Rigidbody2D>(), _moveSpeed);
    }

    private void FixedUpdate()
    {
        float rayLength = 0.5f;

        _hitGround = Physics2D.Raycast(_obstacleRayPoint.position, Vector2.right * _mover.Move(), rayLength);

        if (_hitGround == false || _hitGround.collider.TryGetComponent(out Player _) || _hitGround.collider.TryGetComponent(out Barrier _))
            return;

        _mover.ChangeDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isHaveTarget == false && collision.gameObject.TryGetComponent(out Barrier _))
            _mover.ChangeDirection();
    }
}