using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Animator))]
public class VampiricAura : MonoBehaviour
{
    [SerializeField] private LayerMask _enemiesLayerMask;
    [SerializeField] private float Radius = 4f;

    private readonly float _sizeRatio = 0.43f;

    private Renderer _renderer;
    private Animator _animator;
    private Enemy _target;
    private Collider2D[] _enemies;

    private void Awake()
    {
        float size = Radius * _sizeRatio;

        transform.localScale = new Vector2(size, size);

        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();
    }

    public void Enable()
    {
        _renderer.enabled = true;
        _animator.enabled = true;
    }

    public void Disable()
    {
        _renderer.enabled = false;
        _animator.enabled = false;
    }

    public Enemy SearchTarget()
    {
        _target = null;

        _enemies = Physics2D.OverlapCircleAll(transform.position, Radius, _enemiesLayerMask);

        foreach (Collider2D collider in _enemies)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy) == false)
                continue;

            if (_target == null)
                _target = enemy;
            else
                if (ÑalculateSquareOfDistanceTo(_target) > ÑalculateSquareOfDistanceTo(enemy))
                    _target = enemy;
        }

        return _target;
    }

    private float ÑalculateSquareOfDistanceTo(Enemy enemy)
    {
        return Vector2.SqrMagnitude(transform.position - enemy.transform.position);
    }
}
