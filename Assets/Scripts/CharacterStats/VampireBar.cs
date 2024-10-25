using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VampireBar : MonoBehaviour
{
    [SerializeField] private Vampire _vampire;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _vampire.SkillProgressTimeChanged += ChangeValue;
    }

    private void OnDisable()
    {
        _vampire.SkillProgressTimeChanged -= ChangeValue;
    }

    private void ChangeValue(float fillfactor)
    {
        _slider.value = Mathf.Clamp(_slider.maxValue * fillfactor, 0, _slider.maxValue);
    }
}
