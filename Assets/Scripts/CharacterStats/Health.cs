using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField, Min(1)] public float MaxValue { get; private set; }

    public event Action<float> ValueChanged;
    public event Action<float> ValueChangedFast;

    public float CurrentValue { get; private set; }
    public bool IsZeroValue => CurrentValue <= 0;

    private void Awake()
    {
        CurrentValue = MaxValue;
    }

    public void Decrease(float value)
    {
        if (value > 0)
            ChangeValue(-value);
    }

    public void Increase(float value)
    {
        if (value > 0)
            ChangeValue(value);
    }

    public void DecreaseFast(float value)
    {
        Decrease(value);

        ValueChangedFast?.Invoke(CurrentValue);
    }

    public void IncreaseFast(float value)
    {
        Increase(value);

        ValueChangedFast?.Invoke(CurrentValue);
    }

    private void ChangeValue(float value)
    {
        CurrentValue = Mathf.Clamp(CurrentValue + value, 0, MaxValue);

        ValueChanged?.Invoke(CurrentValue);
    }
}
