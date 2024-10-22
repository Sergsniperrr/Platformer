using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VampiricAura : MonoBehaviour
{
    [field: SerializeField] public float Radius { get; private set; } = 4f;
    [SerializeField] private LayerMask _enemiesLayerMask;

    private readonly float _sizeRatio = 0.43f;

    private Renderer _renderer;
    private Collider2D _target;

    private void Awake()
    {
        float size = Radius * _sizeRatio;

        transform.localScale = new Vector2(size, size);

        _renderer = GetComponent<Renderer>();
    }

    public void Enable()
    {
        _renderer.enabled = true;
    }

    public void Disable()
    {
        _renderer.enabled = false;
    }

    public Enemy SearchTarget()
    {
        _target = Physics2D.OverlapCircle(transform.position, Radius, _enemiesLayerMask);

        if (_target == null)
            return null;

        _target.gameObject.TryGetComponent(out Enemy enemy);

        return enemy;
    }
}
