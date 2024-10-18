using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _ground;

    private float _groundRadius = 0.3f;

    public bool IsGrounded => Physics2D.OverlapCircle(transform.position, _groundRadius, _ground);
}
