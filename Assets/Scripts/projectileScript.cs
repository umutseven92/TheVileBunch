using UnityEngine;
using System.Collections;

public class projectileScript : Photon.PunBehaviour 
{

    // Use this for initialization
    protected virtual void Start()
    {
        Destroy(gameObject, 3);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var colName = other.name;

        if (!colName.StartsWith("ropeAttached") && !colName.StartsWith("slash_0") && !colName.StartsWith("Pickup"))
        {
            Destroy(gameObject);
        }
    }
}
