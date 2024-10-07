using UnityEngine;

public class RotateBlocker : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Mover _mover;

    private void Awake()
    {
        _mover = _player.Mover;
    }

    private void Start()
    {
        _player.Mover.Rotate += FlipBack;
    }

    private void OnDestroy()
    {
        _player.Mover.Rotate -= FlipBack;
    }

    private void FlipBack(float angle)
    {
        transform.Rotate(0, -angle, 0);
    }
}
