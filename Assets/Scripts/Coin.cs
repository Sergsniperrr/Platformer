using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
