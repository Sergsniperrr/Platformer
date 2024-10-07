using UnityEngine;

public class RotateBlocker : MonoBehaviour
{
    [SerializeField] private Mover _mover;

    private void Start()
    {
        _mover.Rotate += FlipBack;
    }

    private void OnDestroy()
    {
        _mover.Rotate -= FlipBack;
    }

    private void FlipBack(float angle)
    {
        transform.Rotate(0, -angle, 0);
    }
}
