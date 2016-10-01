using System;
using UnityEngine;
using System.Collections;

public class localPickupScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Player") || other.name.StartsWith("groundCheck",StringComparison.InvariantCultureIgnoreCase))
        {
            Destroy(gameObject);
        }
    }
}
