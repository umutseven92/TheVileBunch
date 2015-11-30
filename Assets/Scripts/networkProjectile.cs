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
            transform.position = Vector3.Lerp(transform.position, _correctBulletPos, Time.deltaTime * 50);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Our bullets

            // Position
            stream.SendNext(transform.position);
            
            /* Destroyed
            if (this.GetComponentInParent<projectileScript>().gameObject == null)
            {
                stream.SendNext(true);
            }
            else
            {
                stream.SendNext(false);
            }
            */
        }
        else
        {
            // Their bullets

            // Position
            _correctBulletPos = (Vector3)stream.ReceiveNext();

            /*
            if ((bool) stream.ReceiveNext())
            {
                PhotonNetwork.Destroy(gameObject);
            }
            */

        }
    }

}
