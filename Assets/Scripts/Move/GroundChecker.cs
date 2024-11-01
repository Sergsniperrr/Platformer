using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _ground;

    private float _groundRadius = 0.3f;
    private float _intervalOfCheck = 0.1f;
    private float _counter;

    public bool IsGrounded;

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter >= _intervalOfCheck)
        {
            IsGrounded = Physics2D.OverlapCircle(transform.position, _groundRadius, _ground);
            _counter = 0f;
        }
    }
}
