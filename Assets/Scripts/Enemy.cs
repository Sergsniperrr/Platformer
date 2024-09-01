using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = -3f;
    [SerializeField] private Transform _target;

    private Chase _chase;
    private Mover _mover;

    private void Start()
    {
        _mover = new Mover(transform, GetComponent<Rigidbody2D>(), _moveSpeed);
        _chase = new Chase(transform, _target, _mover);
    }

    private void FixedUpdate()
    {
        _chase.PerformMoveActions();
    }

    private void Update()
    {
        _chase.ControlFlipOnSameLevelWithTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TriggerPoint triggerPoint))
            _chase.HandleActionsAtTriggerPoint(triggerPoint);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy sameEnemy))
            _chase.HandleCollisionWithSame(sameEnemy.transform);
    }
}
