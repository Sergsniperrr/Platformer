using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField, Min(1)] public int MaxValue { get; private set; }

    private int _currentValue;

    public event Action<int> ValueChanged;
    public event Action<int> ValueChangedFast;

    public bool IsZeroValue => _currentValue <= 0;

    protected virtual void Awake()
    {
        _currentValue = MaxValue;
    }

    public void Decrease(int value)
    {
        if (value > 0)
        {
            _currentValue -= value;

            if (_currentValue < 0)
            {
                _currentValue = 0;
            }

            ValueChanged?.Invoke(_currentValue);
        }
    }

    public void Increase(int value)
    {
        if (value > 0)
        {
            _currentValue += value;

            if (_currentValue > MaxValue)
                _currentValue = MaxValue;

            ValueChanged?.Invoke(_currentValue);
        }
    }

    public void DecreaseFast(int value)
    {
        Decrease(value);

        ValueChangedFast?.Invoke(_currentValue);
    }

    public void IncreaseFast(int value)
    {
        Increase(value);

        ValueChangedFast?.Invoke(_currentValue);
    }
}
