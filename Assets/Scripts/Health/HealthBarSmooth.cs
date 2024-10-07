using System.Collections;
using UnityEngine;

public class HealthBarSmooth : HealthBar
{
    private float _rateOfChange = 40f;
    private Coroutine _coroutine;

    protected override void ChangeValue(int newValue)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SmoothChangeValue(newValue));
    }

    private IEnumerator SmoothChangeValue(int newValue)
    {
        while (Slider.value != newValue)
        {
            Slider.value = Mathf.MoveTowards(Slider.value, newValue, _rateOfChange * Time.deltaTime);

            yield return null;
        }
    }
}
