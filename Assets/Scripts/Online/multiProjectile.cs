using UnityEngine;
using System.Collections;

public class multiProjectile : projectileScript
{

    protected override void Start()
    {
        base.Start();
        //pView = GetComponentInParent<PhotonView>();
    }

    void OnDestroy()
    {
    }
}
