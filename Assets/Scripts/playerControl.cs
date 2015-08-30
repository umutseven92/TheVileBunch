using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public AudioClip GunShot;
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
    private bool _dead = false;

    private Sprite _health1;
    private Sprite _health2;

    private Sprite _health1Reverse;
    private Sprite _health2Reverse;

    [HideInInspector]
    public string _playerClass;

    private List<playerSelect.Player> _localPlayers;

    private double healthBarMs = 2.000d;
    private bool paused = false;
    private int playerNum = 0;

    private bool first = true;

    // Use this for initialization
    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        _health1 = Resources.Load<Sprite>("threehealth");
        _health2 = Resources.Load<Sprite>("quarterhealth");

        _health1Reverse = Resources.Load<Sprite>("threehealthReverse");
        _health2Reverse = Resources.Load<Sprite>("quarterhealthReverse");

        _healthBarRender = HealthBar.GetComponent<SpriteRenderer>();

        _localPlayers = playerSelect.PlayerList;
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

        _up = Input.GetAxis(Control + "Vertical") > 0.3f;

        CheckHealthBar();
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

    void Shoot()
    {
        var shotTransform = Instantiate(Bullet) as Transform;

        if (FacingRight)
        {
            shotTransform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
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
                    //Instantiate(Explosion, transform.position, transform.rotation);
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
