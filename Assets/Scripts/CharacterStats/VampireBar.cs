using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VampireBar : MonoBehaviour
{
    private Slider _slider;

    public float MaxValue => _slider.maxValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void Increase(float value)
    {
        if (value <= 0f)
            return;

        if (_slider.value + value > _slider.maxValue)
        {
            _slider.value = _slider.maxValue;
            return;
        }

        _slider.value += value;
    }

    public void Decrease(float value)
    {
        if (value <= 0f)
            return;

        if (_slider.value - value < 0)
        {
            _slider.value = 0;
            return;
        }

        _slider.value -= value;
    }
}
