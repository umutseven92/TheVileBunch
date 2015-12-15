using UnityEngine;
using System.Collections;

public class networkPlayer : Photon.PunBehaviour
{
    PhotonTransformView transformView;
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        transformView = GetComponent<PhotonTransformView>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       // transformView.SetSynchronizedValues(new Vector3(rigidBody.velocity.magnitude),0);
    }
}
