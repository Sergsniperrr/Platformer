using UnityEngine;

public class VampiricAuraView : MonoBehaviour
{
    [SerializeField] private VampiricAura _aura;

    private Renderer _renderer;
    private Animator _animator;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _animator = GetComponent<Animator>();

        SetSize(_aura.Radius);
    }

    private void OnEnable()
    {
        _aura.StatusChanged += ChangeSatus;
    }

    private void OnDisable()
    {
        _aura.StatusChanged -= ChangeSatus;
    }

    private void SetSize(float radius)
    {
        float sizeRatio = 0.43f;
        float size = radius * sizeRatio;

        transform.localScale = new Vector2(size, size);
    }

    private void ChangeSatus(bool isEnabled)
    {
        _renderer.enabled = isEnabled;
        _animator.enabled = isEnabled;
    }
}
