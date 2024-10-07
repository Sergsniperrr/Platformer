using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, Min(1)] private int _maxValue = 100;

    public event Action<int> ChangeValue;

    public int MaxValue => _maxValue;
    public int CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = _maxValue;
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            CurrentValue -= damage;

            if (CurrentValue < 0)
                CurrentValue = 0;

            ChangeValue?.Invoke(CurrentValue);
        }
    }

    public void TakeHealing(int healingAmount)
    {
        if (healingAmount > 0)
        {
            CurrentValue += healingAmount;

            if (CurrentValue > _maxValue)
                CurrentValue = _maxValue;

            ChangeValue?.Invoke(CurrentValue);
        }
    }
}
