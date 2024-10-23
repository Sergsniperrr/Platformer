using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _value;

    private float _rateOfChange = 50f;
    private Coroutine _coroutine;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _value.MaxValue;
        _slider.value = _slider.maxValue;
    }

    private void OnEnable()
    {
        _value.ValueChanged += ChangeValue;
        _value.ValueChangedFast += ChangeValueFast;
    }

    private void OnDisable()
    {
        _value.ValueChanged -= ChangeValue;
        _value.ValueChangedFast -= ChangeValueFast;
    }

    private void ChangeValueFast(float newValue)
    {
        _slider.value = newValue;
    }

    private void ChangeValue(float newValue)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SmoothChangeValue(newValue));
    }

    private IEnumerator SmoothChangeValue(float newValue)
    {
        while (_slider.value != newValue)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, newValue, _rateOfChange * Time.deltaTime);

            yield return null;
        }
    }
}
