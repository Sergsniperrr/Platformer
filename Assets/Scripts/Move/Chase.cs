using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Transform _patrolPoint;
    [SerializeField] private LayerMask _players;
    [SerializeField] private float _aggressiveRadius = 5f;
    [SerializeField] private float _delayBeforeChangeTarget = 3f;

    private readonly Vector2 _playerModelOffset = new (0f, 0.736839f);

    private Transform _currentTarget;
    private WaitForSeconds _waitForChangeTarget;
    private Coroutine _coroutineOfChangeTarget;
    private Collider2D _newTarget;

    private void Awake()
    {
        _waitForChangeTarget = new WaitForSeconds(_delayBeforeChangeTarget);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TriggerPoint triggerPoint))
            HandleActionsAtTriggerPoint(triggerPoint);
    }

    private void Update()
    {
        ControlFlipOnSameLevelWithTarget();
        HandleOfChangeTarget();
    }

    private void ControlFlipOnSameLevelWithTarget()
    {
        if (InspectPresenceOrNoTargetOnSameLevel() == false)
            return;

        if (((_currentTarget.position.x - transform.position.x) < 0) == _mover.IsFacingRight)
            _mover.ChangeDirection();
    }

    private void HandleActionsAtTriggerPoint(TriggerPoint triggerPoint)
    {
        Vector2 direction = CalculateDirectionToTarget();
        float maxValueCoordinateX = 2.4f;
        float minOffset = 0.2f;

        direction = SetNormalizedVector(direction, maxValueCoordinateX);

        if (Mathf.Abs(direction.y) <= minOffset)
            direction.y = 0f;
        else
            direction.y /= Mathf.Abs(direction.y);

        direction.x /= Mathf.Abs(direction.x);

        Dictionary<Vector2, WayAction> wayActions = new()
        {
            { new Vector2(-1.00f, 1.00f), triggerPoint.OnTopLeft },
            { new Vector2(1.00f, 1.00f), triggerPoint.OnTopRight },
            { new Vector2(-1.00f, -1.00f), triggerPoint.OnBottomLeft },
            { new Vector2(1.00f, -1.00f), triggerPoint.OnBottomRight },
            { new Vector2(-1.00f, 0.00f), triggerPoint.OnLeft },
            { new Vector2(1.00f, 0.00f), triggerPoint.OnRight }
        };

        if (wayActions.ContainsKey(direction) == false)
            return;

        _mover.ReadWayAction(wayActions[direction]);
    }

    private void ChangeTarget(Transform newTarget)
    {
        if (newTarget != null)
            _currentTarget = newTarget;
        else
            _currentTarget = _patrolPoint;
    }

    private void HandleOfChangeTarget()
    {
        _newTarget = Physics2D.OverlapCircle(transform.position, _aggressiveRadius, _players);

        if (_newTarget == null && _currentTarget == _patrolPoint)
            return;

        if (_newTarget != null)
        {
            if (_coroutineOfChangeTarget != null)
            {
                StopCoroutine(_coroutineOfChangeTarget);
                _coroutineOfChangeTarget = null;
            }

            if (_newTarget.transform != _currentTarget)
                ChangeTarget(_newTarget.transform);
        }
        else
        {
            if (_coroutineOfChangeTarget == null)
                _coroutineOfChangeTarget = StartCoroutine(ChangeTargetAfterDelay());
        }
    }

    private IEnumerator ChangeTargetAfterDelay()
    {
        yield return _waitForChangeTarget;

        ChangeTarget(_patrolPoint);
    }

    private Vector2 SetNormalizedVector(Vector2 direction, float maxValueCoordinateX)
    {        
        float newCoordinateX = Mathf.Clamp(direction.x, -maxValueCoordinateX, maxValueCoordinateX);
        float correctZeroValue = 0.001f;

        if (direction.x == 0f)
            direction.x = correctZeroValue;

        direction.y *= newCoordinateX / direction.x;
        direction.x = newCoordinateX;

        return direction;
    }

    private Vector2 CalculateDirectionToTarget()
    {
        if (_currentTarget == null)
            ChangeTarget(_patrolPoint);

        return (Vector2)(_currentTarget.position - transform.position) - _playerModelOffset;
    }

    private bool InspectPresenceOrNoTargetOnSameLevel()
    {
        if (_currentTarget == null)
            ChangeTarget(_patrolPoint);

        return Mathf.Round(_currentTarget.position.y - transform.position.y - _playerModelOffset.y) == 0f;
    }
}