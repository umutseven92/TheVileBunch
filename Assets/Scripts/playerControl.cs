using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class playerControl : MonoBehaviour
{
    [HideInInspector]
    public bool FacingRight = true;

    [HideInInspector]
    public bool Jump = false;

    [HideInInspector]
    public bool Run = false;

    [HideInInspector]
    public string Control;

    public float MoveForce = 365f;
    public float MaxSpeed = 5f;
    public float JumpForce = 10f;
    public float BulletSpeed = 20f;
    public Transform GroundCheck;
    public Transform GroundCheck2;
    public Transform Bullet;
    public Transform HealthBar;
    public Transform Explosion;
    public Transform BloodSplatter;
    public Transform DeadPlayer;
    public Transform SwordSlash;

    public AudioClip GunShot;
    public AudioClip SlashClip;
    public int Health = 3;
    public int Ammo = 3;

    private double _counter = 0.000d;
    private bool _grounded = false;
    private bool _grounded2 = false;
    private bool _inFrontOfLadder = false;
    private bool _up = false;
    private Rigidbody2D _rb2D;
    private AudioSource _audio;
    private Animator _animator;
    private SpriteRenderer _healthBarRender;
    private SpriteRenderer _sRenderer;
    private bool _dead = false;
    private const float slashOffset = 0.8f;

    private Sprite _health1;
    private Sprite _health2;

    private Sprite _health1Reverse;
    private Sprite _health2Reverse;

    [HideInInspector]
    public string _playerClass;

    private List<playerSelect.Player> _localPlayers;

    private const double healthBarMs = 2.000d;
    private bool paused = false;

    [HideInInspector]
    public int playerNum = 0;

    private bool first = true;

    private GameObject SlashCol;

    private bool _slashing = false;
    private double _slashingCounter = 0.000d;
    private const double _slashingMs = 0.3d;

    private const int pushX = 500;
    private const int pushY = 400;

    private bool hit = false;
    private const double _hitMs = 2.000d;
    private double _hitCounter = 0.000d;

    private const double flash = 0.200d;
    private double flashTimer = 0.000d;

    // Use this for initialization
    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _sRenderer = GetComponent<SpriteRenderer>();

        _health1 = Resources.Load<Sprite>("threehealth");
        _health2 = Resources.Load<Sprite>("quarterhealth");

        _health1Reverse = Resources.Load<Sprite>("threehealthReverse");
        _health2Reverse = Resources.Load<Sprite>("quarterhealthReverse");

        _healthBarRender = HealthBar.GetComponent<SpriteRenderer>();

        _localPlayers = playerSelect.PlayerList;

        SlashCol = Instantiate(SwordSlash.gameObject,
            FacingRight
                ? new Vector3(transform.position.x + slashOffset, transform.position.y, transform.position.y)
                : new Vector3(transform.position.x - slashOffset, transform.position.y, transform.position.y),
            transform.rotation) as GameObject;

    }


    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            if (playerNum == 2 || playerNum == 4)
            {
                Flip();
            }
            first = false;
        }

        _grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _grounded2 = Physics2D.Linecast(transform.position, GroundCheck2.position, 1 << LayerMask.NameToLayer("Ground"));


        if (Input.GetButtonDown(Control + "Jump") && ((_grounded || _grounded2) || _inFrontOfLadder) && !paused)
        {
            Jump = true;
        }

        if (Input.GetButtonDown(Control + "Fire") && !paused && Ammo > 0)
        {
            Shoot();
        }

        if (Input.GetButtonDown(Control + "Slash") && !paused && !_slashing)
        {
            Slash();
        }

        _up = Input.GetAxis(Control + "Vertical") > 0.3f;

        CheckTimers();
        UpdateSlashColPos();
    }

    void UpdateSlashColPos()
    {
        SlashCol.transform.position = FacingRight ? new Vector3(transform.position.x + slashOffset, transform.position.y, transform.position.z) : new Vector3(transform.position.x - slashOffset, transform.position.y, transform.position.z);
    }

    private void CheckHealthBar()
    {
        if (_healthBarRender.enabled)
        {
            _counter += 1 * Time.deltaTime;

            if (_counter >= healthBarMs)
            {
                _healthBarRender.enabled = false;
                _counter = 0.000d;
            }
        }
    }

    private void CheckTimers()
    {
        CheckHealthBar();
        CheckSlashingTimer();
        CheckHitTimer();
    }

    private void CheckSlashingTimer()
    {
        if (_slashing)
        {
            _slashingCounter += 1 * Time.deltaTime;
            if (_slashingCounter >= _slashingMs)
            {
                _slashing = false;
                SlashCol.SendMessage("GetCol", _slashing);
                _slashingCounter = 0.000d;
            }
        }
    }

    private void CheckHitTimer()
    {
        if (hit)
        {
            _hitCounter += 1 * Time.deltaTime;

            if (_hitCounter >= flashTimer)
            {
                flashTimer += flash;
                _sRenderer.enabled = !_sRenderer.enabled;
            }

            if (_hitCounter >= _hitMs)
            {
                hit = false;
                _hitCounter = 0.000d;
                flashTimer = 0.000d;
                _sRenderer.enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis(Control + "Horizontal");

        float v = Input.GetAxis(Control + "Vertical");

        if (h == 0)
        {
            _animator.SetInteger("anim", 0);
        }

        if (_inFrontOfLadder)
        {
            if (!paused)
            {
                _rb2D.gravityScale = 0;
                _rb2D.velocity = Vector3.zero;


                if (v * _rb2D.velocity.y < MaxSpeed)
                {
                    _rb2D.AddForce(Vector2.up * v * MoveForce);
                }

                if (Mathf.Abs(_rb2D.velocity.y) > MaxSpeed)
                {
                    _rb2D.velocity = new Vector2(_rb2D.velocity.x, Mathf.Sign(_rb2D.velocity.y) * MaxSpeed);
                }

            }

        }
        else
        {
            if (!paused)
            {
                _rb2D.gravityScale = 4;

                if (h * _rb2D.velocity.x < MaxSpeed)
                {
                    _rb2D.AddForce(Vector2.right * h * MoveForce);
                }

                if (Mathf.Abs(_rb2D.velocity.x) > MaxSpeed)
                {
                    _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * MaxSpeed, _rb2D.velocity.y);
                }

                if (h != 0)
                {
                    _animator.SetInteger("anim", 1);
                }

            }
        }

        if (h > 0 && !FacingRight && !paused)
        {
            Flip();
        }
        else if (h < 0 && FacingRight && !paused)
        {
            Flip();
        }

        if (Jump)
        {
            if (_inFrontOfLadder)
            {
                _inFrontOfLadder = false;
            }

            // JUMP
            _rb2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;

        }
    }

    void Flip()
    {
        if (FacingRight)
        {
            switch (Health)
            {
                case 2:
                    _healthBarRender.sprite = _health1Reverse;
                    break;
                case 1:
                    _healthBarRender.sprite = _health2Reverse;
                    break;
            }
        }
        else
        {
            switch (Health)
            {
                case 2:
                    _healthBarRender.sprite = _health1;
                    break;
                case 1:
                    _healthBarRender.sprite = _health2;
                    break;
            }
        }

        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.localScale = theScale;

    }

    void Slash()
    {
        _slashing = true;
        SlashCol.SendMessage("GetCol", _slashing);
        _audio.PlayOneShot(SlashClip);
    }

    void Shoot()
    {
        var shotTransform = Instantiate(Bullet) as Transform;

        if (FacingRight)
        {
            shotTransform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            shotTransform.Rotate(0, 180, 0);
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(BulletSpeed, 0);
        }
        else
        {
            shotTransform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(-BulletSpeed, 0);

        }
        _audio.PlayOneShot(GunShot);
        Ammo--;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.name.Equals("ropeAttached") && _up)
        {
            _inFrontOfLadder = true;
        }

        if (other.name.Equals("Bullet(Clone)"))
        {
            Health--;

            // Small push
            if (other.gameObject.transform.position.x < this.transform.position.x)
            {
                _rb2D.AddForce(new Vector2(pushX, pushY));
            }
            else
            {
                _rb2D.AddForce(new Vector2(-pushY, pushY));
            }

            CheckHealth(other);
        }

        if (other.name.StartsWith("slash"))
        {

            if (other.GetComponent<slashScript>().num != playerNum && other.GetComponent<slashScript>().slashing)
            {
                if (!hit)
                {


                    hit = true;
                    Health--;

                    if (Health > 0)
                    {
                        float pX = 0f;

                        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");

                        foreach (var g in go)
                        {
                            if (g.gameObject.GetComponent<playerControl>().playerNum ==
                                other.GetComponent<slashScript>().num)
                            {
                                pX = g.transform.position.x;
                            }
                        }

                        // Small push
                        if (pX < this.transform.position.x)
                        {
                            _rb2D.AddForce(new Vector2(pushX, pushY));
                        }
                        else
                        {
                            _rb2D.AddForce(new Vector2(-pushX, pushY));
                        }
                    }
                    CheckHealth(other);
                }

            }
        }
    }

    void CheckHealth(Collider2D other)
    {

        switch (Health)
        {
            case 2:
                _healthBarRender.sprite = _health1;
                _healthBarRender.enabled = true;
                break;
            case 1:
                _healthBarRender.sprite = _health2;
                _healthBarRender.enabled = true;
                break;
            case 0:
                // DEATH
                _dead = true;
                Instantiate(BloodSplatter, transform.position, transform.rotation);
                GameObject deadPlayer = Instantiate(DeadPlayer.gameObject, transform.position, transform.rotation) as GameObject;

                if (other.transform.position.x < this.transform.position.x)
                {
                    deadPlayer.SendMessage("Die", "left");
                }
                else
                {
                    deadPlayer.SendMessage("Die", "right");
                }

                Destroy(gameObject);
                break;
            default:
                Debug.LogError("Health not between 1 and 4!");
                break;
        }
    }

    void PlayerNumber(int number)
    {
        SetPlayerInfo(number);
    }

    void SetPlayerInfo(int num)
    {
        playerNum = num;
        Control = _localPlayers[num - 1].Control;
        _playerClass = _localPlayers[num - 1].Class;
        SlashCol.SendMessage("GetPlayerNum", playerNum);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached"))
        {
            _inFrontOfLadder = false;
            _up = false;
        }
    }

    void Pause()
    {
        paused = true;
    }

    void UnPause()
    {
        paused = false;
    }
}
