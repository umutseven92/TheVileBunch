using UnityEngine;

public class networkCharacter : Photon.MonoBehaviour
{
    private Vector3 _correctPlayerPos;
    private float _correctLocalScale;

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 20);
            transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Our player
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale.x);
        }
        else
        {
            // Network player
            _correctPlayerPos = (Vector3)stream.ReceiveNext();
            _correctLocalScale = (float)stream.ReceiveNext();
        }
    }

}
