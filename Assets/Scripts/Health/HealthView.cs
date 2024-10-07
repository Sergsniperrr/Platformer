using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] protected Health Health;

    private void OnEnable()
    {
        Health.ChangeValue += ChangeValue;
    }

    private void OnDisable()
    {
        Health.ChangeValue -= ChangeValue;
    }

    protected virtual void ChangeValue(int newValue) { }
}
