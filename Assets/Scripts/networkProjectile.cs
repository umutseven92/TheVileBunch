using UnityEngine;

/// <summary>
/// Online Projectiles
/// </summary>
public class networkProjectile : Photon.MonoBehaviour
{
    private Vector3 _correctBulletPos;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, _correctBulletPos, Time.deltaTime * 20);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Our bullets

            // Position
            stream.SendNext(transform.position);
        }
        else
        {
            // Their bullets

            // Position
            _correctBulletPos = (Vector3)stream.ReceiveNext();

        }
    }

}
