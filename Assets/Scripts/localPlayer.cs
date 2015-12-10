using UnityEngine;
using System.Collections;

public class localPlayer : playerControl
{
    private bool _paused;

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

        if (Input.GetButtonDown(Control + "Slash") && !_paused && !_slashing && !_aiming)
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
    }

    protected override void FixedUpdate()
    {
        if (_paused)
        {
            return;
        }

        base.FixedUpdate();

        if (_inFrontOfLadder)
        {
            _rb2D.gravityScale = 0;
            _rb2D.velocity = Vector3.zero;

            if (!_aiming)
            {
                if (_vertical * _rb2D.velocity.y < MaxSpeed)
                {
                    _rb2D.AddForce(Vector2.up * _vertical * MoveForce);
                }

                if (Mathf.Abs(_rb2D.velocity.y) > MaxSpeed)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, Mathf.Sign(_rb2D.velocity.y) * MaxSpeed);
                }
            }
        }
        else
        {
            _rb2D.gravityScale = 4;

            if (!_aiming)
            {
                if (_horizontal * _rb2D.velocity.x < MaxSpeed)
                {
                    _rb2D.AddForce(Vector2.right * _horizontal * MoveForce);
                }

                if (Mathf.Abs(_rb2D.velocity.x) > MaxSpeed)
                {
                    _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * MaxSpeed, _rb2D.velocity.y);
                }
            }
        }

        if (_horizontal > 0 && !FacingRight && !_paused)
        {
            Flip();
        }
        else if (_horizontal < 0 && FacingRight && !_paused)
        {
            Flip();
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

}
