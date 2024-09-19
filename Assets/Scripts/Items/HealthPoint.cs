using UnityEngine;

public class HealthPoint : Item
{
    [SerializeField] private int _powerOfRegeneration = 20;

    public int PowerOfRegeneration => _powerOfRegeneration;
}
