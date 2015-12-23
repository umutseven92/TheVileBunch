﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class onlinePlayer : playerControl
{
    [HideInInspector]
    public string OnlinePlayerName;

    [HideInInspector]
    public bool Enabled; // Control for online, enable after spawn

    public Text OnlineNameText;

    void Start()
    {
        pView = GetComponentInParent<PhotonView>();
    }

    public override void Flip()
    {
        base.Flip();
        FlipLeftRight(OnlineNameText.transform);
    }

    protected override void Awake()
    {
        base.Awake();
        MultiSetPlayer();
    }

    protected override void Update()
    {
        base.Update();

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

        if (Input.GetButtonDown(Control + "Slash") && !_slashing && !_aiming && Enabled)
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


        if (Enabled)
        {
            HandleAnimations();
        }

        if (_aiming && Enabled)
        {
            DrawLine();
        }
    }

    protected override void FixedUpdate()
    {
        if (!Enabled)
        {
            return;
        }

        base.FixedUpdate();

    }

    private void MultiSetPlayer()
    {
        playerNum = 1;
        Control = "j1";
        _playerClass = "The Cowboy";
        OnlinePlayerName = PlayerPrefs.GetString(global.PlayerName);
        OnlineNameText.text = OnlinePlayerName;
        _slashCol.SendMessage("GetPlayerNum", playerNum);
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
        _hit = true;

        LowerHealth(BulletDamage);

        // Small push
        _rb2D.AddForce(Other.gameObject.transform.position.x < this.transform.position.x
            ? new Vector2(PushX, PushY)
            : new Vector2(-PushY, PushY));

        CheckHealth(Other);

    }

    public void HitBySlash()
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

            foreach (
                var g in
                    go.Where(
                        g =>
                            g.gameObject.GetComponent<playerControl>().playerNum ==
                            Other.GetComponent<slashScript>().num))
            {
                pX = g.transform.position.x;
            }

            // Small push
            _rb2D.AddForce(pX < transform.position.x
                ? new Vector2(PushX, PushY)
                : new Vector2(-PushX, PushY));
        }
        CheckHealth(Other);
    }

    private Collider2D Other;
    private PhotonView pView;

    void OnTriggerEnter2D(Collider2D other)
    {
        Other = other;

        // Rope
        if (other.name.StartsWith("ropeAttached") && _up)
        {
            _inFrontOfLadder = true;
        }

        // Bullet
        if (other.name.StartsWith("Bullet(Clone)"))
        {
            if (!_hit)
            {
                pView.RPC("BulletHitRPC", PhotonTargets.All, pView.viewID);
            }
        }

        // Sword slash
        if (other.name.StartsWith("slash"))
        {
            if (other.GetComponent<slashScript>().num != playerNum && other.GetComponent<slashScript>().slashing)
            {
                if (!_hit)
                {
                    pView.RPC("SlashHitRPC", PhotonTargets.All, pView.viewID);
                }
            }
        }

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

        var pView = GetComponentInParent<PhotonView>();
        pView.RPC("ShootRPC", PhotonTargets.All, pView.viewID, bXPos, bYPos, bXSpeed, bYSpeed);

        bXPos = 0;
        bYPos = 0;
        bXSpeed = 0;
        bYSpeed = 0;
    }

    protected void Slash()
    {
        var pView = GetComponentInParent<PhotonView>();
        pView.RPC("SlashRPC", PhotonTargets.All, pView.viewID);
    }
}