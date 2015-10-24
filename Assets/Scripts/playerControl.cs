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

    public int StartingAmmo = 3; // Starting ammo
    public int MaxAmmo = 3; // Maximum ammo a player can have
    public int StartingHealth = 3; // Starting health
    public int MaxHealth = 3; // Maximum health a player can have
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
    public double _healedMs = 2.000d; // Health bar visibility after healed
    public double _gunLightMs = 0.100d; // How log does gun light stays
    public double flash = 0.200d; // Player invulnerability flash frequency
    public int pushX = 500; // How far in the X plane player flies when hit
    public int pushY = 400; // How far in the Y plane player flies when hit
    public int AmmoPickup = 3; // How much ammo does ammo pickup give
    public int HealthPickup = 3; // How much health does health pickup give
    public float DirectionLock = 0.2f;
    public double _aimMs = 0.2d;
    public Transform GroundCheck;
    public Transform GroundCheck2;
    public Transform Bullet;
    public Transform Explosion;
    public Transform BloodSplatter;
    public Transform DeadPlayer;
    public Transform SwordSlash;
    public AudioClip GunShot;
    public AudioClip SlashClip;
    public AudioClip HealthClip;
    public AudioClip AmmoClip;
    public AudioClip GunCockClip;
    public Light GunLight;
    #endregion

    #region Private Values

    private int Health;
    private int Ammo;
    private bool _grounded = false;
    private bool _grounded2 = false;
    private bool _inFrontOfLadder = false;
    private bool _up = false;
    private bool _dead = false;
    private bool first = true;
    private bool paused = false;
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
    private bool healed = false;
    private double _hitCounter = 0.000d;
    private double _healedCounter = 0.000d;
    private double flashTimer = 0.000d;
    private bool _gunLight = false;
    private double _gunLightCounter = 0.000d;
    private float _horizontal;
    private bool BulletUp;
    private bool BulletDown;
    private bool BulletRight;
    private bool BulletLeft;
    private bool aiming;
    private bool softAim;
    private double _aimCounter = 0.000d;

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

        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;

        GunLight.enabled = false;
        GunLight.intensity = 0;

        Ammo = StartingAmmo;
        Health = StartingHealth;
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

        if (Input.GetButtonDown(Control + "Fire") && !paused && Ammo > 0)
        {
            softAim = true;
        }

        if (Input.GetButtonDown(Control + "Jump") && ((_grounded || _grounded2) || _inFrontOfLadder) && !paused && !aiming)
        {
            Jump = true;
        }

        if (Input.GetButtonUp(Control + "Fire") && !paused && Ammo > 0)
        {
            Shoot();
            aiming = false;
            softAim = false;
        }

        if (Input.GetButtonDown(Control + "Slash") && !paused && !_slashing && !aiming)
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

        if (aiming)
        {
            _animator.SetInteger("anim", 0);
        }
        if (_slashing)
        {
            // Slash
            _animator.SetInteger("anim", 3);
        }
        if (!_slashing && jumping)
        {
            // Jump
            _animator.SetInteger("anim", 2);
        }
        if (!_slashing && !jumping && _horizontal == 0)
        {
            // Idle
            _animator.SetInteger("anim", 0);
        }
        if (!_slashing && !jumping && ((_horizontal > MovementLock && _horizontal > 0) || (_horizontal < -MovementLock && _horizontal < 0)) && !aiming)
        {
            // Running
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

        if (v > DirectionLock && v > 0)
        {
            BulletUp = true;
            BulletDown = false;
        }
        else if (v < -DirectionLock && v < 0)
        {
            BulletUp = false;
            BulletDown = true;
        }

        if (v < DirectionLock && v > -DirectionLock)
        {
            BulletUp = false;
            BulletDown = false;
        }

        if (h > DirectionLock && h > 0)
        {
            BulletRight = true;
            BulletLeft = false;
        }
        else if (h < -DirectionLock && h < 0)
        {
            BulletRight = false;
            BulletLeft = true;
        }

        if (h < DirectionLock && h > -DirectionLock)
        {
            BulletRight = false;
            BulletLeft = false;
        }

        if (_inFrontOfLadder)
        {
            _rb2D.gravityScale = 0;
            _rb2D.velocity = Vector3.zero;

            if (!aiming)
            {
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
            _rb2D.gravityScale = 4;

            if (!aiming)
            {
                if (h * _rb2D.velocity.x < MaxSpeed)
                {
                    _rb2D.AddForce(Vector2.right * h * MoveForce);
                }

                if (Mathf.Abs(_rb2D.velocity.x) > MaxSpeed)
                {
                    _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * MaxSpeed, _rb2D.velocity.y);
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

            _rb2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;
        }

        if (aiming)
        {
            _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
        }

        DrawLine();
    }

    void DrawLine()
    {

    }

    // Player collided with..
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
        }

        // Health pickup
        if (other.name.StartsWith("Health"))
        {
            _audio.PlayOneShot(HealthClip);
            healed = true;
            GiveHealth(HealthPickup);
        }

        // Sword slash
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

    void GiveHealth(int health)
    {
        Health += health;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            _healthSlider.value = _healthSlider.maxValue;
        }
        else
        {
            _healthSlider.value += health;
        }

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
        CheckHealedTimer();
        CheckAimTimer();
    }

    private void CheckHealthBar()
    {
        if (hit || healed)
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

    private void CheckHealedTimer()
    {
        if (healed)
        {
            _healedCounter += 1 * Time.deltaTime;

            if (_healedCounter >= _healedMs)
            {
                healed = false;
                _healedCounter = 0.000d;
            }

        }
    }

    private void CheckAimTimer()
    {
        if (softAim)
        {
            _aimCounter += 1 * Time.deltaTime;

            if (_aimCounter >= _aimMs)
            {
                softAim = false;
                aiming = true;
                _aimCounter = 0.000d;
                _audio.PlayOneShot(GunCockClip);
            }

        }
        else
        {
            _aimCounter = 0.000d;
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

        float bXPos = 0f;
        float bYPos = 0f;
        float bXSpeed = 0f;
        float bYSpeed = 0f;
        int bRotation = 0;

        if (!BulletUp && !BulletDown && !BulletRight && !BulletLeft)
        {
            if (FacingRight)
            {
                BulletRight = true;
            }
            else
            {
                BulletLeft = true;
            }
        }

        if (BulletRight)
        {
            bXSpeed = BulletSpeed;
            bXPos = 0.5f;
            bRotation += 180;
        }
        else if (BulletLeft)
        {
            bXSpeed = -BulletSpeed;
            bXPos = -0.5f;
        }

        if (BulletUp)
        {
            bYSpeed = BulletSpeed;
            bYPos = 0.5f;

            if (BulletRight)
            {
                bRotation += 45 - 180;
            }
            if (BulletLeft)
            {
                bRotation += 45;
            }
            else
            {
                bRotation += 90;
            }

        }
        else if (BulletDown)
        {
            bYSpeed = -BulletSpeed;
            bYPos = -0.5f;

            if (BulletRight)
            {
                bRotation += 45 + 90;
            }
            if (BulletLeft)
            {
                bRotation += 45 - 90;
            }
            else
            {
                bRotation -= 90;
            }
        }

        shotTransform.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);
        shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bXSpeed, bYSpeed);

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
