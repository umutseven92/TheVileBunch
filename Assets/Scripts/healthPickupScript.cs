using UnityEngine;
using System.Collections;

public class healthPickupScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
