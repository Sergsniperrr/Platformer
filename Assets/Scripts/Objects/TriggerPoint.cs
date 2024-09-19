using System;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    [Header("Enemy's reaction to target direction")]
    [SerializeField] private WayAction _onTopLeft;
    [SerializeField] private WayAction _onTopRight;
    [SerializeField] private WayAction _onBottomLeft;
    [SerializeField] private WayAction _onBottomRight;
    [SerializeField] private WayAction _onLeft;
    [SerializeField] private WayAction _onRight;

    public WayAction OnTopLeft => _onTopLeft;
    public WayAction OnTopRight => _onTopRight;
    public WayAction OnBottomLeft => _onBottomLeft;
    public WayAction OnBottomRight => _onBottomRight;
    public WayAction OnLeft => _onLeft;
    public WayAction OnRight => _onRight;
}

[Serializable]
public class WayAction
{
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isJumpLow;

    public bool IsFacingRight => _isFacingRight;
    public bool IsJump => _isJump;
    public bool IsJumpLow => _isJumpLow;
}
