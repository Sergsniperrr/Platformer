using System;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Animator))]
public class VampiricAura : MonoBehaviour
{
    [SerializeField] private LayerMask _enemiesLayerMask;

    [field: SerializeField] public float Radius { get; private set; } = 4f;

    private Enemy _target;
    private Collider2D[] _enemies;

    public event Action<bool> StatusChanged;

    public void Enable()
    {
        StatusChanged?.Invoke(true);
    }

    public void Disable()
    {
        StatusChanged?.Invoke(false);
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
                if (CalculateSquareOfDistanceTo(_target) > CalculateSquareOfDistanceTo(enemy))
                    _target = enemy;
        }

        return _target;
    }

    private float CalculateSquareOfDistanceTo(Enemy enemy)
    {
        return Vector2.SqrMagnitude(transform.position - enemy.transform.position);
    }
}
