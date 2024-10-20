using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Item : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
