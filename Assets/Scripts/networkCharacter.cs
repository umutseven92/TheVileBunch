using UnityEngine;
using System.Collections;

public class networkCharacter : Photon.MonoBehaviour
{
    private Vector3 _correctPlayerPos;
    private Quaternion _correctPlayerRot;

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, _correctPlayerRot, Time.deltaTime * 5);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Our player
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player
            _correctPlayerPos = (Vector3)stream.ReceiveNext();
            _correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
