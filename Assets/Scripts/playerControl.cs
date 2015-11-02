using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using XInputDotNetPure;

public class playerControl : MonoBehaviour
{
    #region Public Values

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

    public bool Enabled;
    public bool Multi = false;
    public int StartingAmmo = 3; // Starting ammo
    public int MaxAmmo = 3; // Maximum ammo a player can have
    public int StartingHealth = 3; // Starting health
    public int MaxHealth = 3; // Maximum health a player can have
    public int BulletDamage = 2; // How much damage one bullet causes
    public int SwordDamage = 1; // How much damage a sword slash causes
    public float MoveForce = 365f; // Player move force
    public float MaxSpeed = 5f; // Player max speed
    public float JumpForce = 10f; // Player jump force
    public float BulletSpeed = 20f; // Speed of the bullet
    public float MovementLock = 0.2f; // Analog stick movement start value
    public float GunLightSpeed = 0.30f; // How fast does gun light appear
    public float SlashOffset = 0.8f; // How far the player slashes
    public double SlashingMs = 0.3d; // How long does slashing take
    public double HitMs = 2.000d; // Invulnerability timer after getting hit
    public double HealedMs = 2.000d; // Health bar visibility after healed
    public double GunLightMs = 0.200d; // How log does gun light stays
    public double Flash = 0.200d; // Player invulnerability flash frequency
    public int PushX = 500; // How far in the X plane player flies when hit
    public int PushY = 400; // How far in the Y plane player flies when hit
    public int AmmoPickup = 3; // How much ammo does ammo pickup give
    public int HealthPickup = 3; // How much health does health pickup give
    public float DirectionLock = 0.2f; // Analog direction start value
    public double AimMs = 0.15d; // How long to press before player starts aiming

    public Transform GroundCheck;
    public Transform GroundCheck2;
    public Transform Bullet;
    public Transform Explosion;
    public Transform BloodSplatter;
    public Transform DeadPlayer;
    public Transform SwordSlash;
    public Transform Line;
    public AudioClip GunShot;
    public AudioClip SlashClip;
    public AudioClip HealthClip;
    public AudioClip AmmoClip;
    public AudioClip GunCockClip;
    public Light GunLight;
    #endregion

    #region Private Values

    private int _health;
    private int _ammo;
    private bool _grounded;
    private bool _grounded2;
    private bool _inFrontOfLadder;
    private bool _up;
    private bool _dead;
    private bool _first = true;
    private bool _paused;
    private Rigidbody2D _rb2D;
    private AudioSource _audio;
    private Animator _animator;
    private SpriteRenderer _sRenderer;
    private Slider _healthSlider;
    private List<playerSelect.Player> _localPlayers;
    private GameObject _slashCol;
    private bool _slashing;
    private double _slashingCounter;
    private bool _hit;
    private bool _healed;
    private double _hitCounter;
    private double _healedCounter;
    private double _flashTimer;
    private bool _gunLight;
    private double _gunLightCounter;
    private float _horizontal;
    private bool _bulletUp;
    private bool _bulletDown;
    private bool _bulletRight;
    private bool _bulletLeft;
    private bool _aiming;
    private bool _softAim;
    private double _aimCounter;
    private bool _aimCanceled;
    private Color _playerColor;

    #endregion

    #region Properties

    private Transform _line;
    public Transform AimLine
    {
        get
        {
            if (_line == null)
            {
                _line = Instantiate(Line);
            }

            return _line;
        }
    }

    #endregion

    void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _sRenderer = GetComponent<SpriteRenderer>();
        _healthSlider = GetComponentInChildren<Slider>();

        _localPlayers = playerSelect.PlayerList;

        _slashCol = Instantiate(SwordSlash.gameObject,
            FacingRight
                ? new Vector3(transform.position.x + SlashOffset, transform.position.y, transform.position.y)
                : new Vector3(transform.position.x - SlashOffset, transform.position.y, transform.position.y),
            transform.rotation) as GameObject;

        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;

        GunLight.enabled = false;
        GunLight.intensity = 0;

        _ammo = StartingAmmo;
        _health = StartingHealth;
    }

    void Update()
    {

        if (_first)
        {
            if (Multi)
            {
                MultiSetPlayer();
            }

            if (playerNum == 1)
            {
                _playerColor = new Color(0f, 0.5f, 0.5f, 1f);
            }
            else if (playerNum == 2)
            {
                Flip();
                _playerColor = new Color(0, 0.5f, 0, 1f);
            }
            else if (playerNum == 3)
            {
                _playerColor = new Color(0, 0.5f, 0.5f, 1f);
            }
            else if (playerNum == 4)
            {
                Flip();
                _playerColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            _sRenderer.color = _playerColor;
            _first = false;
        }

        _grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _grounded2 = Physics2D.Linecast(transform.position, GroundCheck2.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown(Control + "Fire") && !_paused && _ammo > 0 && Enabled)
        {
            _softAim = true;
        }

        if (Input.GetButtonDown(Control + "Jump") && ((_grounded || _grounded2) || _inFrontOfLadder) && !_paused && !_aiming && Enabled)
        {
            Jump = true;
        }

        if (Input.GetButtonUp(Control + "Fire") && !_paused && _ammo > 0 && Enabled)
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

        if (Input.GetButtonDown(Control + "Slash") && !_paused && !_slashing && !_aiming && Enabled)
        {
            Slash();
        }

        if (Input.GetButtonDown(Control + "Slash") && !_paused && !_slashing && _aiming && Enabled)
        {
            _aiming = false;
            AimLine.GetComponent<SpriteRenderer>().enabled = false;
            _aimCanceled = true;
        }

        _up = Input.GetAxis(Control + "Vertical") > 0.3f;

        CheckTimers();
        UpdateSlashColPos();

        if (Enabled)
        {
            HandleAnimations();
        }

        if (_aiming && Enabled)
        {
            DrawLine();
        }
    }

    private void HandleAnimations()
    {
        var jumping = !_grounded && !_grounded2;

        if (_aiming)
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
        if (!_slashing && !jumping && ((_horizontal > MovementLock && _horizontal > 0) || (_horizontal < -MovementLock && _horizontal < 0)) && !_aiming)
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

        if (_paused)
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
            _bulletUp = true;
            _bulletDown = false;
        }
        else if (v < -DirectionLock && v < 0)
        {
            _bulletUp = false;
            _bulletDown = true;
        }

        if (v < DirectionLock && v > -DirectionLock)
        {
            _bulletUp = false;
            _bulletDown = false;
        }

        if (h > DirectionLock && h > 0)
        {
            _bulletRight = true;
            _bulletLeft = false;
        }
        else if (h < -DirectionLock && h < 0)
        {
            _bulletRight = false;
            _bulletLeft = true;
        }

        if (h < DirectionLock && h > -DirectionLock)
        {
            _bulletRight = false;
            _bulletLeft = false;
        }

        if (_inFrontOfLadder && Enabled)
        {
            _rb2D.gravityScale = 0;
            _rb2D.velocity = Vector3.zero;

            if (!_aiming)
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
            if (Enabled)
            {
                _rb2D.gravityScale = 4;

                if (!_aiming)
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
        }

        if (h > 0 && !FacingRight && !_paused && Enabled)
        {
            Flip();
        }
        else if (h < 0 && FacingRight && !_paused && Enabled)
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

        if (_aiming)
        {
            _rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
        }

    }

    void DrawLine()
    {
        AimLine.GetComponent<SpriteRenderer>().enabled = true;

        var line = AimLine;
        var bXPos = 0f;
        var bYPos = 0f;
        var bRotation = 0f;

        if (!_bulletUp && !_bulletDown && !_bulletRight && !_bulletLeft)
        {
            if (FacingRight)
            {
                _bulletRight = true;
            }
            else
            {
                _bulletLeft = true;
            }
        }

        if (_bulletRight)
        {
            bXPos = 0.5f;
            bRotation += 180;
        }
        else if (_bulletLeft)
        {
            bXPos = -0.5f;
        }

        if (_bulletUp)
        {
            bYPos = 0.5f;

            if (_bulletRight)
            {
                bRotation += 45 - 180;
            }
            if (_bulletLeft)
            {
                bRotation += 45;
            }
            else
            {
                bRotation += 90;
            }

        }
        else if (_bulletDown)
        {
            bYPos = -0.5f;

            if (_bulletRight)
            {
                bRotation += 45 + 90;
            }
            if (_bulletLeft)
            {
                bRotation += 45 - 90;
            }
            else
            {
                bRotation -= 90;
            }
        }

        line.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);

        line.eulerAngles = new Vector3(0, 0, -bRotation);
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
            _ammo += AmmoPickup;
            if (_ammo > MaxAmmo)
            {
                _ammo = MaxAmmo;
            }
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

                    if (_health > 0)
                    {
                        float pX = 0f;

                        var go = GameObject.FindGameObjectsWithTag("Player");

                        foreach (var g in go.Where(g => g.gameObject.GetComponent<playerControl>().playerNum == other.GetComponent<slashScript>().num))
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

    void LowerHealth(int damage)
    {
        _health -= damage;
        _healthSlider.value -= damage;
    }

    void GiveHealth(int health)
    {
        _health += health;

        if (_health > MaxHealth)
        {
            _health = MaxHealth;
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
        _slashCol.transform.position = FacingRight ? new Vector3(transform.position.x + SlashOffset, transform.position.y, transform.position.z) : new Vector3(transform.position.x - SlashOffset, transform.position.y, transform.position.z);
    }

    private void CheckTimers()
    {
        CheckHealthBar();
        CheckSlashingTimer();
        CheckHitTimer();
        CheckGunLightTimer();
        CheckHealedTimer();
        CheckAimTimer();
        CheckVibrationTimer();
    }

    private void CheckHealthBar()
    {
        if (_hit || _healed)
        {
            _healthSlider.GetComponentInParent<Canvas>().enabled = true;
        }
        else
        {
            _healthSlider.GetComponentInParent<Canvas>().enabled = false;
        }
    }

    #region Timers

    private void CheckSlashingTimer()
    {
        if (_slashing)
        {
            _slashingCounter += 1 * Time.deltaTime;
            if (_slashingCounter >= SlashingMs)
            {
                _slashing = false;
                _slashCol.SendMessage("GetCol", _slashing);
                _slashingCounter = 0.000d;
            }
        }
    }

    private void CheckGunLightTimer()
    {
        if (_gunLight)
        {
            _gunLightCounter += 1 * Time.deltaTime;

            if (_gunLightCounter >= GunLightMs / 2)
            {
                GunLight.intensity -= GunLightSpeed;
            }
            else
            {
                GunLight.intensity += GunLightSpeed;
            }

            if (_gunLightCounter >= GunLightMs)
            {
                GunLight.enabled = false;
                _gunLight = false;
                _gunLightCounter = 0.000d;
            }
        }
    }

    private void CheckHitTimer()
    {
        if (_hit)
        {
            _hitCounter += 1 * Time.deltaTime;

            if (_hitCounter >= _flashTimer)
            {
                _flashTimer += Flash;
                _sRenderer.enabled = !_sRenderer.enabled;
            }

            if (_hitCounter >= HitMs)
            {
                _hit = false;
                _hitCounter = 0.000d;
                _flashTimer = 0.000d;
                _sRenderer.enabled = true;
            }
        }
    }

    private void CheckHealedTimer()
    {
        if (_healed)
        {
            _healedCounter += 1 * Time.deltaTime;

            if (_healedCounter >= HealedMs)
            {
                _healed = false;
                _healedCounter = 0.000d;
            }

        }
    }

    private void CheckAimTimer()
    {
        if (_softAim)
        {
            _aimCounter += 1 * Time.deltaTime;

            if (_aimCounter >= AimMs)
            {
                _softAim = false;
                _aiming = true;
                _aimCounter = 0.000d;
                _audio.PlayOneShot(GunCockClip);
            }

        }
        else
        {
            _aimCounter = 0.000d;
        }
    }

    #endregion

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

        _slashCol.SendMessage("GetCol", _slashing);
        _audio.PlayOneShot(SlashClip);
    }


    void Shoot()
    {
        var shotTransform = Instantiate(Bullet) as Transform;

        float bXPos = 0f;
        float bYPos = 0f;
        float bXSpeed = 0f;
        float bYSpeed = 0f;

        if (!_bulletUp && !_bulletDown && !_bulletRight && !_bulletLeft)
        {
            if (FacingRight)
            {
                _bulletRight = true;
            }
            else
            {
                _bulletLeft = true;
            }
        }

        if (_bulletRight)
        {
            bXSpeed = BulletSpeed;
            bXPos = 0.5f;
        }
        else if (_bulletLeft)
        {
            bXSpeed = -BulletSpeed;
            bXPos = -0.5f;
        }

        if (_bulletUp)
        {
            bYSpeed = BulletSpeed;
            bYPos = 0.5f;
        }
        else if (_bulletDown)
        {
            bYSpeed = -BulletSpeed;
            bYPos = -0.5f;
        }

        shotTransform.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);
        shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bXSpeed, bYSpeed);

        _audio.PlayOneShot(GunShot);
        GunLight.enabled = true;
        _gunLight = true;
        _ammo--;
        VibrateGamePad(playerNum);

        Debug.Log(playerNum);
    }

    private double vibrationCounter = 0.000d;
    public double vibrationMs = 0.500d;
    private bool vibrating = false;

    void VibrateGamePad(int player)
    {
        switch (player)
        {
            case 1:
                GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
                break;
            case 2:
                GamePad.SetVibration(PlayerIndex.Two, 1f, 1f);
                break;
            case 3:
                GamePad.SetVibration(PlayerIndex.Three, 1f, 1f);
                break;
            case 4:
                GamePad.SetVibration(PlayerIndex.Four, 1f, 1f);
                break;
        }

        vibrating = true;
    }

    void CheckVibrationTimer()
    {
        if (vibrating)
        {
            vibrationCounter += 1 * Time.deltaTime;

            if (vibrationCounter >= vibrationMs)
            {
                vibrationCounter = 0.000d;

                switch (playerNum)
                {
                    case 1:
                        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                        break;
                    case 2:
                        GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
                        break;
                    case 3:
                        GamePad.SetVibration(PlayerIndex.Three, 0f, 0f);
                        break;
                    case 4:
                        GamePad.SetVibration(PlayerIndex.Four, 0f, 0f);
                        break;
                }

                vibrating = false;
            }
        }
    }

    void CheckHealth(Collider2D other)
    {
        if (_health <= 0)
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

            deadPlayer.SendMessage("SetColor", _playerColor);
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

        _slashCol.SendMessage("GetPlayerNum", playerNum);
    }

    void MultiSetPlayer()
    {
        playerNum = 1;
        Control = "j1";
        _playerClass = "The Cowboy";

        _slashCol.SendMessage("GetPlayerNum", playerNum);
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
