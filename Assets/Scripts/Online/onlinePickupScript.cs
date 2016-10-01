using System;
using UnityEngine;
using System.Collections;

public class onlinePickupScript : Photon.MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.StartsWith("Player") || other.name.StartsWith("groundCheck",StringComparison.InvariantCultureIgnoreCase))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
