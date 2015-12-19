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
    private float _fraction;

    void Update()
    {
        if (!photonView.isMine)
        {
            var net = photonView.transform.GetComponent<onlinePlayer>();

            net.AmmoText.enabled = true;
            net.HealthSlider.GetComponentInParent<Canvas>().enabled = true;

            // We get 10 updates per sec. sometimes a few less or one or two more, depending on variation of lag.
            // Due to that we want to reach the correct position in a little over 100ms. This way, we usually avoid a stop.
            // Lerp() gets a fraction value between 0 and 1. This is how far we went from A to B.
            // Our fraction variable would reach 1 in 100ms if we multiply deltaTime by 10.
            // We want it to take a bit longer, so we multiply with 9 instead.
            _fraction = _fraction + Time.deltaTime * 9;
            //photonView.GetComponent<Rigidbody2D>().position = Vector3.Lerp(transform.position, _correctPlayerPos, _fraction);
            photonView.transform.position = Vector3.Lerp(transform.position, _correctPlayerPos, Time.deltaTime * 20);

            SetOrientation(net);
            net.OnlineNameText.text = _onlineName;
            net.Health = _health;
            net.Ammo = _ammo;
        }
        Debug.Log(PhotonNetwork.GetPing());
    }

    private void SetOrientation(onlinePlayer net)
    {
        FlipSliderLeftRight(net);

        transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
        net.OnlineNameText.transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);
        net.AmmoText.transform.localScale = new Vector3(_correctLocalScale, transform.localScale.y, transform.localScale.z);

        if (_correctLocalScale < 0)
        {
            net._slashCol.transform.position = new Vector3(transform.position.x - net.SlashOffset, transform.position.y, transform.position.z);
        }
        else
        {
            net._slashCol.transform.position = new Vector3(transform.position.x + net.SlashOffset, transform.position.y, transform.position.z);
        }

    }

    private void FlipSliderLeftRight(onlinePlayer net)
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            var ply = transform.GetComponent<onlinePlayer>();

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

            _fraction = 0;
        }
    }

    [PunRPC]
    public void ShootRPC(int pId, float bXPos, float bYPos, float bXSpeed, float bYSpeed)
    {
        RPCBase(pId).OnlineShoot(bXSpeed, bYSpeed, bXPos, bYPos);
    }

    [PunRPC]
    public void SlashRPC(int pId)
    {
        RPCBase(pId).OnlineSlash();
    }

    [PunRPC]
    public void SlashHitRPC(int pId)
    {
        RPCBase(pId).HitBySlash();
    }

    [PunRPC]
    public void BulletHitRPC(int pId)
    {
        RPCBase(pId).HitByBullet();
    }

    private static onlinePlayer RPCBase(int pId)
    {
        var pView = PhotonView.Find(pId);
        return pView.GetComponentInParent<onlinePlayer>();
    }

}

