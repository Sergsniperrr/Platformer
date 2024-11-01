using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Item : MonoBehaviour
{
    private void Awake()
    {
        Collider2D _collider = GetComponent<Collider2D>();

        _collider.isTrigger = true;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
