using System.Collections;
using UnityEngine;

public class EnemyMover : Mover
{
    private float _stunDuration = 0.5f;
    private WaitForSeconds _waitForStunEnd;
    private Coroutine _coroutinePush;
    private bool _isJump;
    private bool _isJumpLow;
    private bool _isNeedFlip;

    protected override void Awake()
    {
        _waitForStunEnd = new WaitForSeconds(_stunDuration);
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (CanMove)
            PerformMoveActions();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy sameEnemy))
            HandleCollisionWithSame(sameEnemy.transform);
    }

    public override void PushBack(float force)
    {
        if (_coroutinePush != null)
            StopCoroutine(_coroutinePush);

        Push(force, IsFacingRight == false);

        _coroutinePush = StartCoroutine(PushWhenTakingDamage());
    }

    public void HandleCollisionWithSame(Transform sameEnemy)
    {
        Vector2 direction = sameEnemy.position - transform.position;

        _isNeedFlip = direction.x > 0;
        _isJumpLow = direction.y < 0;
    }

    public void ReadWayAction(WayAction wayAction)
    {
        _isNeedFlip = wayAction.IsFacingRight != IsFacingRight;
        _isJump = wayAction.IsJump;
        _isJumpLow = wayAction.IsJumpLow;
    }

    private void PerformMoveActions()
    {
        Move();

        if (_isNeedFlip)
        {
            ChangeDirection();
            _isNeedFlip = false;
        }

        if (_isJump)
        {
            Jump();
            _isJump = false;
            _isJumpLow = false;
        }
        else if (_isJumpLow)
        {
            JumpLow();
            _isJumpLow = false;
        }
    }

    private IEnumerator PushWhenTakingDamage()
    {
        DisableMoving();

        yield return _waitForStunEnd;

        EnableMoving();
    }
}
