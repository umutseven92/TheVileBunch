using UnityEngine;
using System.Collections;

public class multiProjectile : projectileScript
{
    private PhotonView pView;

    protected override void Start()
    {
        base.Start();
        pView = GetComponentInParent<PhotonView>();
    }

    void OnDestroy()
    {
    }
}
