using UnityEngine;
using System.Collections;

public class ammoPickupScript : Photon.MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Player"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
