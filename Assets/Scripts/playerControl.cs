using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

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

    [HideInInspector]
    public int playerNum = 0;

    [HideInInspector]
    public string _playerClass;

    #region Public Values

    public int Health = 3;  // Player health
    public int Ammo = 3; // Ammo count
    public int bulletDamage = 2; // How much damage one bullet causes
    public int swordDamage = 1; // How much damage a sword slash causes
    public float MoveForce = 365f; // Player move force
    public float MaxSpeed = 5f; // Player max speed
    public float JumpForce = 10f; // Player jump force
    public float BulletSpeed = 20f; // Speed of the bullet
    public float MovementLock = 0.2f; // Analog stick movement start value
    public float GunLightSpeed = 0.30f; // How fast does gun light appear
    public float slashOffset = 0.8f; // How far the player slashes
    public double _slashingMs = 0.3d; // How long does slashing take
    public double _hitMs = 2.000d; // Invulnerability timer after getting hit
    public double _gunLightMs = 0.100d; // How log does gun light stays
    public double flash = 0.200d; // Player invulnerability flash frequency
    public int pushX = 500; // How far in the X plane player flies when hit
    public int pushY = 400; // How far in the Y plane player flies when hit

    #endregion

    public Transform GroundCheck;
    public Transform GroundCheck2;
    public Transform Bullet;
    public Transform Explosion;
    public Transform BloodSplatter;
    public Transform DeadPlayer;
    public Transform SwordSlash;
    public AudioClip GunShot;
    public AudioClip SlashClip;
    public Light GunLight;

    #region Private Values

    private bool _grounded = false;
    private bool _grounded2 = false;
    private bool _inFrontOfLadder = false;
    private bool _up = false;
    private bool _dead = false;
    private bool first = true;
    private bool paused = false;
    private int _maxHealth;
    private Rigidbody2D _rb2D;
    private AudioSource _audio;
    private Animator _animator;
    private SpriteRenderer _sRenderer;
    private Slider _healthSlider;
    private List<playerSelect.Player> _localPlayers;
    private double _counter = 0.000d;
    private GameObject SlashCol;
    private bool _slashing = false;
    private double _slashingCounter = 0.000d;
    private bool hit = false;
    private double _hitCounter = 0.000d;
    private double flashTimer = 0.000d;
    private bool _gunLight = false;
    private double _gunLightCounter = 0.000d;
    private float _horizontal;

    #endregion

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _sRenderer = GetComponent<SpriteRenderer>();
        _healthSlider = GetComponentInChildren<Slider>();

        _localPlayers = playerSelect.PlayerList;

        SlashCol = Instantiate(SwordSlash.gameObject,
            FacingRight
                ? new Vector3(transform.position.x + slashOffset, transform.position.y, transform.position.y)
                : new Vector3(transform.position.x - slashOffset, transform.position.y, transform.position.y),
            transform.rotation) as GameObject;

        _maxHealth = Health;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;

        GunLight.enabled = false;
        GunLight.intensity = 0;

    }

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
        HandleAnimations();

    }

    private void HandleAnimations()
    {
        var jumping = !_grounded && !_grounded2;

        if (_slashing)
        {
            _animator.SetInteger("anim", 3);
        }
        if (!_slashing && jumping)
        {
            _animator.SetInteger("anim", 2);
        }
        if (!_slashing && !jumping && _horizontal == 0)
        {
            _animator.SetInteger("anim", 0);
        }
        if (!_slashing && !jumping && _horizontal != 0)
        {
            _animator.SetInteger("anim", 1);
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis(Control + "Horizontal");
        float v = Input.GetAxis(Control + "Vertical");

        _horizontal = h;

        if (paused)
        {
            return;
        }

        // Stop movement if value is below movement lock
        if ((h < MovementLock && h > 0) || (h > -MovementLock && h < 0))
        {
            h = 0;
        }

        if (_inFrontOfLadder)
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
        else
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

            _rb2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached") && _up)
        {
            _inFrontOfLadder = true;
        }

        if (other.name.Equals("Bullet(Clone)"))
        {
            if (!hit)
            {
                hit = true;
                LowerHealth(bulletDamage);

                // Small push
                _rb2D.AddForce(other.gameObject.transform.position.x < this.transform.position.x
                    ? new Vector2(pushX, pushY)
                    : new Vector2(-pushY, pushY));

                CheckHealth(other);
            }
        }

        if (other.name.Equals("FallCollider"))
        {
            Die(other);
        }

        if (other.name.StartsWith("slash"))
        {
            if (other.GetComponent<slashScript>().num != playerNum && other.GetComponent<slashScript>().slashing)
            {
                if (!hit)
                {
                    hit = true;
                    LowerHealth(swordDamage);

                    if (Health > 0)
                    {
                        float pX = 0f;

                        var go = GameObject.FindGameObjectsWithTag("Player");

                        foreach (var g in go.Where(g => g.gameObject.GetComponent<playerControl>().playerNum == other.GetComponent<slashScript>().num))
                        {
                            pX = g.transform.position.x;
                        }

                        // Small push
                        _rb2D.AddForce(pX < transform.position.x
                            ? new Vector2(pushX, pushY)
                            : new Vector2(-pushX, pushY));
                    }
                    CheckHealth(other);
                }

            }
        }
    }

    void LowerHealth(int damage)
    {
        Health -= damage;
        _healthSlider.value -= damage;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached"))
        {
            _inFrontOfLadder = false;
            _up = false;
        }
    }

    void UpdateSlashColPos()
    {
        SlashCol.transform.position = FacingRight ? new Vector3(transform.position.x + slashOffset, transform.position.y, transform.position.z) : new Vector3(transform.position.x - slashOffset, transform.position.y, transform.position.z);
    }

    private void CheckTimers()
    {
        CheckHealthBar();
        CheckSlashingTimer();
        CheckHitTimer();
        CheckGunLightTimer();
    }

    private void CheckHealthBar()
    {
        if (hit)
        {
            _healthSlider.GetComponentInParent<Canvas>().enabled = true;
        }
        else
        {
            _healthSlider.GetComponentInParent<Canvas>().enabled = false;
        }
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


    private void CheckGunLightTimer()
    {
        if (_gunLight)
        {
            _gunLightCounter += 1 * Time.deltaTime;

            if (_gunLightCounter >= _gunLightMs / 2)
            {
                GunLight.intensity -= GunLightSpeed;
            }
            else
            {
                GunLight.intensity += GunLightSpeed;
            }

            if (_gunLightCounter >= _gunLightMs)
            {
                GunLight.enabled = false;
                _gunLight = false;
                _gunLightCounter = 0.000d;
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

    void Flip()
    {
        if (FacingRight)
        {
            _healthSlider.direction = Slider.Direction.RightToLeft;
        }
        else
        {
            _healthSlider.direction = Slider.Direction.LeftToRight;
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
        GunLight.enabled = true;
        _gunLight = true;
        Ammo--;
    }


    void CheckHealth(Collider2D other)
    {
        if (Health <= 0)
        {
            Die(other);
        }
    }

    void Die(Collider2D other)
    {
        _dead = true;
        Instantiate(BloodSplatter, transform.position, transform.rotation);

        if (!other.name.Equals("FallCollider"))
        {
            GameObject deadPlayer = Instantiate(DeadPlayer.gameObject, transform.position, transform.rotation) as GameObject;

            deadPlayer.SendMessage("Die", other.transform.position.x < this.transform.position.x ? "left" : "right");
        }

        Destroy(gameObject);
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

    void Pause()
    {
        paused = true;
    }

    void UnPause()
    {
        paused = false;
    }
}
