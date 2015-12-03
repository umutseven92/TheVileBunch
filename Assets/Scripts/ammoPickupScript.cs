using UnityEngine;
using System.Collections;

public class ammoPickupScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Player"))
        {
            Destroy(gameObject);
        }
    }
}
