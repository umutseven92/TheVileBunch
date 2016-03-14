using UnityEngine;
using System.Collections;

public class networkProjectile : Photon.MonoBehaviour {

    private Vector3 _correctBulletPos;

    void Update()
    {
        if (!photonView.isMine)
        {
            photonView.transform.position = Vector3.Lerp(transform.position, _correctBulletPos, Time.deltaTime * 20);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            _correctBulletPos = (Vector3)stream.ReceiveNext();
        }
    }


}
