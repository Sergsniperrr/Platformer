using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPoint : MonoBehaviour
{
    [SerializeField] private int _powerOfRegeneration = 20;

    public int PowerOfRegeneration => _powerOfRegeneration;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
