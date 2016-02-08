using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;
using UnityEngine.UI;

public class onlinePlayer : playerControl
{
    [HideInInspector]
    public string OnlinePlayerName;

    [HideInInspector]
    public bool Enabled; // Control for online, enable after spawn

    public Text OnlineNameText;

    private Collider2D _other;
    private PhotonView _pView;

    [HideInInspector]
    public int Ping;

    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    void Start()
    {
        _pView = GetComponentInParent<PhotonView>();
    }

    public override void Flip()
    {
        base.Flip();
        FlipLeftRight(OnlineNameText.transform);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if (!string.IsNullOrEmpty(Control))
        {
            if (Input.GetButtonDown(Control + "Fire") && Ammo > 0 && Enabled)
            {
                _softAim = true;
            }

            if (Input.GetButtonDown(Control + "Jump") && (Grounded || _inFrontOfLadder) && !_aiming && Enabled)
            {
                Jump = true;
            }

            if (Input.GetButtonUp(Control + "Fire") && Ammo > 0 && Enabled)
            {
                if (_aimCanceled)
                {
                    _aimCanceled = false;
                    return;
                }
                Shoot();
                AimLine.GetComponent<SpriteRenderer>().enabled = false;
                _aiming = false;
                _softAim = false;
            }

            if (Input.GetButtonDown(Control + "Slash") && !_slashing && !_aiming && Enabled && !_slashDelay)
            {
                Slash();
            }

            if (Input.GetButtonDown(Control + "Slash") && !_slashing && _aiming && Enabled)
            {
                _aiming = false;
                AimLine.GetComponent<SpriteRenderer>().enabled = false;
                _aimCanceled = true;
            }

            _up = Input.GetAxis(Control + "Vertical") > 0.3f;
        }

        if (Enabled)
        {
            HandleAnimations();
        }

        if (_aiming && Enabled)
        {
            DrawLine();
        }

        Ping = PhotonNetwork.GetPing();
    }

    protected override void FixedUpdate()
    {
        if (!Enabled)
        {
            return;
        }

        base.FixedUpdate();

    }

    public void OnlineShoot(float bXSpeed, float bYSpeed, float bXPos, float bYPos)
    {
        Shoot(bXSpeed, bYSpeed, bXPos, bYPos);
    }

    public void OnlineSlash()
    {
        Slash(5);
    }

    public void HitByBullet()
    {
        if (!_hit)
        {
            _hit = true;

            LowerHealth(BulletDamage);

            // Small push
            _rb2D.AddForce(_other.gameObject.transform.position.x < this.transform.position.x
                ? new Vector2(PushX, PushY)
                : new Vector2(-PushY, PushY));

            CheckHealth(_other);
            Log.InfoFormat("{0} shot at client.", this.OnlinePlayerName);
        }
    }

    public void HitBySlash()
    {
        if (!_hit)
        {
            _hit = true;
            LowerHealth(SwordDamage);

            if (Health > 0)
            {
                float pX = 0f;

                var go = GameObject.FindGameObjectsWithTag("Player");

                var gList = new List<GameObject>(go);
                gList.RemoveAt(0);
                go = gList.ToArray();

                // BUG HERE
                foreach (
                    var g in
                        go.Where(
                            g =>
                                g.gameObject.GetComponent<playerControl>().playerNum ==
                                _other.GetComponent<slashScript>().num))
                {
                    pX = g.transform.position.x;
                }

                // Small push
                _rb2D.AddForce(pX < transform.position.x
                    ? new Vector2(PushX, PushY)
                    : new Vector2(-PushX, PushY));
            }
            CheckHealth(_other);

            Log.InfoFormat("{0} stabbed at client.", this.OnlinePlayerName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _other = other;

        // Rope
        if (other.name.StartsWith("ropeAttached") && _up)
        {
            _inFrontOfLadder = true;
        }

        /*
        // Bullet
        if (other.name.StartsWith("Bullet(Clone)"))
        {
            if (!_hit)
            {
                Log.InfoFormat("Player {0}({1}) shot in client", _pView.viewID, OnlinePlayerName);
                //_pView.RPC("BulletHitRPC", PhotonTargets.All, _pView.viewID);
            }
        }

        // Sword slash
        if (other.name.StartsWith("slash"))
        {
            if (other.GetComponent<slashScript>().num != playerNum && other.GetComponent<slashScript>().slashing)
            {
                if (!_hit)
                {
                    Log.InfoFormat("Player {0}({1}) stabbed in client", _pView.viewID, OnlinePlayerName);
                    //_pView.RPC("SlashHitRPC", PhotonTargets.All, _pView.viewID);
                }
            }
        }
        */
        // Fall
        if (other.name.StartsWith("FallCollider"))
        {
            Die(other);
        }

        // Ammo pickup
        if (other.name.StartsWith("Ammo"))
        {
            _audio.PlayOneShot(AmmoClip);
            Ammo += AmmoPickup;
            if (Ammo > MaxAmmo)
            {
                Ammo = MaxAmmo;
            }

            _ammoChanged = true;
            _ammoCounter = 0.000d;
        }

        // Health pickup
        if (other.name.StartsWith("Health"))
        {
            _audio.PlayOneShot(HealthClip);
            _healed = true;
            GiveHealth(HealthPickup);
        }

    }

    protected override void Shoot()
    {
        base.Shoot();

        OnlinebXPos = bXPos;
        OnlinebYPos = bYPos;
        OnlinebXSpeed = bXSpeed;
        OnlinebYSpeed = bYSpeed;

        _pView.RPC("ShootRPC", PhotonTargets.All, _pView.viewID, bXPos, bYPos, bXSpeed, bYSpeed);

        bXPos = 0;
        bYPos = 0;
        bXSpeed = 0;
        bYSpeed = 0;
    }

    protected void Slash()
    {
        _pView.RPC("SlashRPC", PhotonTargets.All, _pView.viewID);
    }


}
