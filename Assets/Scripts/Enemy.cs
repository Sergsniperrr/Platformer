using UnityEngine;

public class Enemy : Mover
{
    [SerializeField] private Transform _obstacleRayPoint;

    private RaycastHit2D _hitGround;
    private bool _isHaveTarget = false;

    private void Start()
    {
        Speed = -3f;
        Rigidbody = GetComponent<Rigidbody2D>();
        Direction = new Vector2(Speed, Rigidbody.velocity.y);
    }

    private void FixedUpdate()
    {
        Direction.y = Rigidbody.velocity.y;
        Rigidbody.velocity = Direction;
        _hitGround = Physics2D.Raycast(_obstacleRayPoint.position, Vector2.right * Direction, 0.5f);

        if (_hitGround == false || _hitGround.collider.TryGetComponent(out Player _) || _hitGround.collider.TryGetComponent(out Barrier _))
            return;

        Debug.Log(_hitGround.collider.name);
        ChangeMoveDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isHaveTarget == false && collision.gameObject.TryGetComponent(out Barrier _))
            ChangeMoveDirection();
    }

    private void ChangeMoveDirection()
    {
        int directionChanger = -1;

        Speed *= directionChanger;
        Direction.x = Speed;

        Flip();
    }
}