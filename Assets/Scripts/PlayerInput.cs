using UnityEngine;

public class PlayerInput
{
    private const string Horizontal = nameof(Horizontal);

    public bool IsJumpKeyPress => Input.GetKeyDown(KeyCode.Space);
    public float VelocityOnX => Input.GetAxis(Horizontal);
}