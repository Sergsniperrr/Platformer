using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField, Min(1)] public float MaxValue { get; private set; }

    public event Action<float> ValueChanged;
    public event Action<float> ValueChangedFast;

    public float CurrentValue { get; private set; }
    public bool IsZeroValue => CurrentValue <= 0;

    protected virtual void Awake()
    {
        CurrentValue = MaxValue;
    }

    public void Decrease(float value)
    {
        if (value > 0)
        {
            CurrentValue -= value;

            if (CurrentValue < 0)
                CurrentValue = 0;

            ValueChanged?.Invoke(CurrentValue);
        }
    }

    public void Increase(float value)
    {
        if (value > 0)
        {
            CurrentValue += value;

            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;

            ValueChanged?.Invoke(CurrentValue);
        }
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
}
