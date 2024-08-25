using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    protected Rigidbody2D Rigidbody;
    protected Vector2 Direction;
    protected bool IsFacingRight;
    protected float Speed;

    protected void Flip()
    {
        float maxRotateAngle = 180f;

        IsFacingRight = !IsFacingRight;

        transform.Rotate(0f, maxRotateAngle, 0f);
    }
}
