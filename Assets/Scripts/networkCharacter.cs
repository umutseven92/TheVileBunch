using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Online Character
/// </summary>
public class networkCharacter : Photon.MonoBehaviour
{
    private Vector3 _correctPlayerPos;
    private float _correctLocalScale;
    private string _onlineName;
    private int _health;
    private int _ammo;
    private bool _shooting;
    private float _bXPos;
    private float _bYPos;
    private float _bXSpeed;
    private float _bYSpeed;

    void Update()
    {
        if (!photonView.isMine)
        {
            var net = photonView.transform.GetComponent<playerControl>();

            net.AmmoText.enabled = true;
            net.HealthSlider.GetComponentInParent<Canvas>().enabled = true;
            photonView.transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 20);
            SetOrientation(net);
            net.OnlineNameText.text = _onlineName;
            net.Health = _health;
            net.Ammo = _ammo;

            if (_shooting)
            {
                net.OnlineShoot(_bXSpeed, _bYSpeed, _bXPos, _bYPos);
                _shooting = false;
            }
        }
    }

    private void SetOrientation(playerControl net)
    {
        FlipSliderLeftRight(net);

        transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
        net.OnlineNameText.transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
        net.AmmoText.transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
    }

    private void FlipSliderLeftRight(playerControl net)
    {
        if (_correctLocalScale < 0)
        {
            net.HealthSlider.direction = Slider.Direction.RightToLeft;

        }
        else
        {
            net.HealthSlider.direction = Slider.Direction.LeftToRight;
        }
    }

    [PunRPC]
    public void OnlineShoot(int pId, float bXPos, float bYPos, float bXSpeed, float bYSpeed)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<playerControl>().OnlineShoot(bXSpeed,bYSpeed,bXPos, bYPos);
        Debug.Log("BANG");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            var ply = transform.GetComponent<playerControl>();

            // Our player

            // Position
            stream.SendNext(transform.position);

            // Flip rotation
            stream.SendNext(transform.localScale.x);

            // Online name
            stream.SendNext(PlayerPrefs.GetString(global.PlayerName));

            // Health
            stream.SendNext(ply.Health);

            // Ammo
            stream.SendNext(ply.Ammo);

            // Shooting
            /*
            stream.SendNext(ply.Shooting);

            if (ply.Shooting)
            {
                stream.SendNext(ply.OnlinebXPos);
                stream.SendNext(ply.OnlinebYPos);
                stream.SendNext(ply.OnlinebXSpeed);
                stream.SendNext(ply.OnlinebYSpeed);
            }
            */
        }
        else
        {
            // Network player

            // Position
            _correctPlayerPos = (Vector3)stream.ReceiveNext();

            // Flip rotation
            _correctLocalScale = (float)stream.ReceiveNext();

            // Online name
            _onlineName = (string)stream.ReceiveNext();

            // Health
            _health = (int)stream.ReceiveNext();

            // Ammo
            _ammo = (int)stream.ReceiveNext();

            // Shooting
            /*
            _shooting = (bool)stream.ReceiveNext();

            Debug.Log(_shooting);

            if (_shooting)
            {
                _bXPos = (float)stream.ReceiveNext();
                _bYPos = (float)stream.ReceiveNext();
                _bXSpeed = (float)stream.ReceiveNext();
                _bYSpeed = (float)stream.ReceiveNext();
            }
            */
        }
    }
}
