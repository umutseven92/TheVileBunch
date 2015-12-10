using UnityEngine;
using UnityEngine.UI;

public class onlinePlayer : playerControl
{
    [HideInInspector]
    public string OnlinePlayerName;

    [HideInInspector]
    public bool Enabled; // Control for online, enable after spawn

    public Text OnlineNameText;

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
        base.FixedUpdate();

        if (_inFrontOfLadder && Enabled)
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
            if (Enabled)
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
        }

        if (_horizontal > 0 && !FacingRight && Enabled)
        {
            Flip();
        }
        else if (_horizontal < 0 && FacingRight && Enabled)
        {
            Flip();
        }
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
