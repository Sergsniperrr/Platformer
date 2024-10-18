using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField, Min(1)] public int MaxValue { get; private set; } = 100;

    public event Action<int> ChangeValue;

    public int CurrentValue { get; private set; }

    private void Awake()
    {
        CurrentValue = MaxValue;
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

            if (CurrentValue > MaxValue)
                CurrentValue = MaxValue;

            ChangeValue?.Invoke(CurrentValue);
        }
    }
}
