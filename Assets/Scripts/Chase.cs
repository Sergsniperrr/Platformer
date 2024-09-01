using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase
{
    private readonly Vector2 _playerModelOffset = new (0f, 0.736839f);

    private Transform _chaser;
    private Transform _target;
    private Mover _mover;

    private bool _isJump;
    private bool _isJumpLow;
    private bool _isNeedFlip;

    public Chase(Transform chaser, Transform target, Mover mover)
    {
        _chaser = chaser;
        _target = target;
        _mover = mover;
    }

    private Vector2 DirectionToTarget => ((Vector2)(_target.position - _chaser.position) - _playerModelOffset).normalized;
    private bool IsSameLevelWithTarget => Mathf.Round(_target.position.y - _chaser.position.y - _playerModelOffset.y) == 0f;

    public void PerformMoveActions()
    {
        _mover.Move();

        if (_isNeedFlip)
            _isNeedFlip = _mover.ChangeDirection();

        if (_isJump)
        {
            _isJump = _mover.Jump();
            _isJumpLow = false;
        }
        else if (_isJumpLow)
        {
            _isJumpLow = _mover.JumpLow();
        }
    }

    public void ControlFlipOnSameLevelWithTarget()
    {
        if (IsSameLevelWithTarget)
            _isNeedFlip = ((_target.position.x - _chaser.position.x) < 0) == _mover.IsFacingRight;
    }

    public void HandleActionsAtTriggerPoint(TriggerPoint triggerPoint)
    {
        WayAction wayAction;
        Vector2 direction = DirectionToTarget;
        float minOffset = 0.2f;

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
        Vector2 direction = sameEnemy.position - _chaser.position;

        _isNeedFlip = direction.x > 0;
        _isJumpLow = direction.y < 0;
    }
}
