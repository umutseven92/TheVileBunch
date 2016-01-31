using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class localPlayer : playerControl
{
    private bool _paused;

    private List<playerSelect.Player> _localPlayers;

    protected override void Awake()
    {
        base.Awake();

        _localPlayers = playerSelect.PlayerList;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown(Control + "Fire") && !_paused && Ammo > 0)
        {
            _softAim = true;
        }

        if (Input.GetButtonDown(Control + "Jump") && (Grounded || _inFrontOfLadder) && !_paused && !_aiming)
        {
            Jump = true;
        }

        if (Input.GetButtonUp(Control + "Fire") && !_paused && Ammo > 0)
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

        if (Input.GetButtonDown(Control + "Slash") && !_paused && !_slashing && !_aiming && !_slashDelay)
        {
            Slash();
        }

        if (Input.GetButtonDown(Control + "Slash") && !_paused && !_slashing && _aiming)
        {
            _aiming = false;
            AimLine.GetComponent<SpriteRenderer>().enabled = false;
            _aimCanceled = true;
        }

        _up = Input.GetAxis(Control + "Vertical") > 0.3f;

        HandleAnimations();

        if (_aiming)
        {
            DrawLine();
        }

        // _paused = _spawned;
    }

    protected override void FixedUpdate()
    {
        if (_paused)
        {
            return;
        }

        base.FixedUpdate();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
                _hit = true;

                LowerHealth(BulletDamage);

                // Small push
                _rb2D.AddForce(other.gameObject.transform.position.x < this.transform.position.x
                    ? new Vector2(PushX, PushY)
                    : new Vector2(-PushY, PushY));

                CheckHealth(other);
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

        // Sword slash
        if (other.name.StartsWith("slash"))
        {
            if (other.GetComponent<slashScript>().num != playerNum && other.GetComponent<slashScript>().slashing)
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

                        foreach (
                            var g in
                                go.Where(
                                    g =>
                                        g.gameObject.GetComponent<playerControl>().playerNum ==
                                        other.GetComponent<slashScript>().num))
                        {
                            pX = g.transform.position.x;
                        }

                        // Small push
                        _rb2D.AddForce(pX < transform.position.x
                            ? new Vector2(PushX, PushY)
                            : new Vector2(-PushX, PushY));
                    }
                    CheckHealth(other);
                }
            }
        }

    }

    protected override void Shoot()
    {
        base.Shoot();

        var shotTransform = Instantiate(Bullet) as Transform;
        shotTransform.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);

        shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bXSpeed, bYSpeed);

        bXPos = 0;
        bYPos = 0;
        bXSpeed = 0;
        bYSpeed = 0;

        _audio.PlayOneShot(GunShot);
        GunLight.enabled = true;
        _gunLight = true;
        Ammo--;
        _ammoChanged = true;
        _ammoCounter = 0.000d;

        AmmoText.enabled = true;

        VibrateGamePad(playerNum);
    }

    protected void Slash()
    {
        _slashing = true;

        _slashCol.SendMessage("GetCol", _slashing);
        _audio.PlayOneShot(SlashClip);
    }
    void Pause()
    {
        _paused = true;
    }

    void UnPause()
    {
        _paused = false;
    }
    void PlayerNumber(int num)
    {
        playerNum = num;
        Control = _localPlayers[num - 1].Control;

        _playerClass = _localPlayers[num - 1].Class;

        _slashCol.SendMessage("GetPlayerNum", playerNum);
    }

}
