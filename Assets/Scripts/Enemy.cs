using UnityEngine;

public class Enemy : Mover
{
    [SerializeField] private Transform _obstacleRayPoint;

    private RaycastHit2D _hitGround;

    private void FixedUpdate()
    {
        Direction.y = Rigidbody.velocity.y;
        Rigidbody.velocity = Direction;
        _hitGround = Physics2D.Raycast(_obstacleRayPoint.position, Vector2.right * Direction, 0.5f);

        if (_hitGround == false || _hitGround.collider.CompareTag("Player"))
            return;

        ChangeMoveDirection();
    }

    private void Start()
    {
        Speed = -3f;
        Rigidbody = GetComponent<Rigidbody2D>();
        Direction = new Vector2(Speed, Rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
