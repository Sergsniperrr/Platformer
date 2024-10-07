using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _patrolPoint;

    private readonly Vector2 _playerModelOffset = new (0f, 0.736839f);

    private bool _isJump;
    private bool _isJumpLow;
    private bool _isNeedFlip;

    public Transform CurrentTarget { get; private set; }

    public void PerformMoveActions()
    {
        _mover.Move();

        if (_isNeedFlip)
        {
            _mover.ChangeDirection();
            _isNeedFlip = false;
        }

        if (_isJump)
        {
            _mover.Jump();
            _isJump = false;
            _isJumpLow = false;
        }
        else if (_isJumpLow)
        {
            _mover.JumpLow();
            _isJumpLow = false;
        }
    }

    public void ControlFlipOnSameLevelWithTarget()
    {
        if (InspectPresenceOrNoTargetOnSameLevel())
            _isNeedFlip = ((CurrentTarget.position.x - transform.position.x) < 0) == _mover.IsFacingRight;
    }

    public void HandleActionsAtTriggerPoint(TriggerPoint triggerPoint)
    {
        WayAction wayAction;
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

        wayAction = wayActions[direction];

        _isNeedFlip = wayAction.IsFacingRight != _mover.IsFacingRight;

        _isJump = wayAction.IsJump;
        _isJumpLow = wayAction.IsJumpLow;
    }

    public void HandleCollisionWithSame(Transform sameEnemy)
    {
        Vector2 direction = sameEnemy.position - transform.position;

        _isNeedFlip = direction.x > 0;
        _isJumpLow = direction.y < 0;
    }

    public void ChangeTarget(Transform newTarget)
    {
        if (newTarget != null)
            CurrentTarget = newTarget;
        else
            CurrentTarget = _patrolPoint;
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
        if (CurrentTarget == null)
            ChangeTarget(_patrolPoint);

        return (Vector2)(CurrentTarget.position - transform.position) - _playerModelOffset;
    }

    private bool InspectPresenceOrNoTargetOnSameLevel()
    {
        if (CurrentTarget == null)
            ChangeTarget(_patrolPoint);

        return Mathf.Round(CurrentTarget.position.y - transform.position.y - _playerModelOffset.y) == 0f;
    }
}
