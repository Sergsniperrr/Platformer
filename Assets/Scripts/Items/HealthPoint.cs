using UnityEngine;

public class HealthPoint : Item
{
    [field: SerializeField] public int PowerOfRegeneration { get; private set; } = 20;
}
