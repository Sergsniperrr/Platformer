using UnityEngine;

public class RotateBlocker : MonoBehaviour
{
    [SerializeField] protected Mover Mover;

    private void Start()
    {
        Mover.Rotate += FlipBack;
    }

    private void OnDestroy()
    {
        Mover.Rotate -= FlipBack;
    }

    protected virtual void FlipBack(float angle)
    {
        transform.Rotate(0, -angle, 0);
    }
}
