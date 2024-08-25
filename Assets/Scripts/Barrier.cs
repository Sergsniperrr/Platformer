using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Barrier : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
}
