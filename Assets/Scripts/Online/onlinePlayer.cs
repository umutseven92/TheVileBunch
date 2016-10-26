using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using XInputDotNetPure;

public class onlinePlayer :NetworkBehaviour 
{

	[HideInInspector]
	public bool FacingRight = true;

	[HideInInspector]
	public bool AbleToJump
	{
		get { return JumpCount > 0 && !_jumped; }
	}

	[HideInInspector]
	public bool Run = false;

	[HideInInspector]
	public string Control;

	[HideInInspector]
	public int playerNum = 0;

	[HideInInspector]
	public string _playerClass;

	[HideInInspector]
	public virtual int Health { get; set; }

	[HideInInspector]
	public int Ammo;

	[HideInInspector]
	public Slider HealthSlider;

	[HideInInspector]
	public float OnlinebXSpeed;

	[HideInInspector]
	public float OnlinebYSpeed;

	[HideInInspector]
	public float OnlinebXPos;

	[HideInInspector]
	public float OnlinebYPos;

	[HideInInspector]
	public GameObject _slashCol;

	[HideInInspector]
	public int JumpCount;

	private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	public int Spawn = 3;
	public int StartingAmmo = 3; //!< Starting ammo
	public int MaxAmmo = 3; //!< Maximum ammo a player can have
	public int StartingHealth = 3; //!< Starting health
	public int MaxHealth = 3; //!< Maximum health a player can have
	public int BulletDamage = 2; //!< How much damage one bullet causes
	public int SwordDamage = 1; //!< How much damage a sword slash causes
	public float MoveForce = 365f; //!< Player move force
	public float MaxSpeed = 5f; //!< Player max speed
	public float JumpForce = 10f; //!< Player jump force
	public float BulletSpeed = 20f; //!< Speed of the bullet
	public float MovementLock = 0.2f; //!< Analog stick movement start value
	public float GunLightSpeed = 0.30f; //!< How fast does gun light appear
	public float SlashOffset = 0.5f; //!< How far the player slashes
	public float GunOffset = 0.5f; //!< How far the player shoots 
	public double SlashingMs = 0.3d; //!< How long does slashing take
	public double ShootingMs = 0.3d; //!< How long does shooting take
	public double HitMs = 2.000d; //!< Invulnerability timer after getting hit
	public double HealedMs = 2.000d; //!< Health bar visibility after healed
	public double GunLightMs = 0.200d; //!< How log does gun light stays
	public double Flash = 0.200d; //!< Player invulnerability flash frequency
	public int PushX = 500; //!< How far in the X plane player flies when hit
	public int PushY = 400; //!< How far in the Y plane player flies when hit
	public int AmmoPickup = 3; //!< How much ammo does ammo pickup give
	public int HealthPickup = 3; //!< How much health does health pickup give
	public float DirectionLock = 0.2f; //!< Analog direction start value
	public double AimMs = 0.15d; //!< How long to press before player starts aiming
	public double VibrationMs = 0.500d; //!< How long to vibrate after shooting
	public int MouseAimDeadZone = 50; //!< Mouse deadzone for aimline snapping
	public double AmmoMs = 2.000d; //!< Ammo counter visibility after shooting
	public float GravityScale = 4; //!< The amount of gravity inflicted on the player. Use this for ladders & ropes
	public int MaxJumpCount = 2; //!< How many times you can jump
	public float SecondJumpMultiplier = 1.50f; //!< How bigger the second jump is
	public double JumpDelayMs = 0.5d; //!< Delay between two jumps in a double jump (so the user cant spam it)

	public Transform GroundCheck;
	public Transform Bullet;
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
	public Text AmmoText;
	public Canvas SpawnCanvas;
	public Sprite HealthThree;
	public Sprite HealthTwo;
	public Sprite HealthOne;
	public Image SpawnImage;

	protected bool _inFrontOfLadder;
	protected bool _up;
	private bool _dead;
	private bool _first = true;
	protected Rigidbody2D _rb2D;
	protected AudioSource _audio;
	private Animator _animator;
	private SpriteRenderer _sRenderer;
	private List<playerSelect.Player> _localPlayers;
	protected bool _slashing;
	protected bool _shooting;
	private double _shootingCounter;
	private double _slashingCounter;
	protected bool _hit;
	protected bool _hitByMelee;
	protected bool _hitByBullet;
	protected bool _healed;
	private double _hitCounter;
	private double _healedCounter;
	private double _flashTimer;
	protected bool _gunLight;
	private double _gunLightCounter;
	protected double _ammoCounter;
	protected float _horizontal;
	protected float _vertical;
	private BoxCollider2D _groundCheckCollider;

	protected bool _spawned;
	private double _spawnedCounter;
	public double _spawnedMs = 2.000d;

	protected bool _bulletRight;
	protected bool _bulletLeft;
	protected bool _aiming;
	protected bool _softAim;
	private double _aimCounter;
	protected bool _aimCanceled;
	private Color _playerColor;
	private double vibrationCounter;
	private bool vibrating;

	protected bool _jumped;
	private double jumpDelayCounter;

	protected bool Grounded;
	protected bool _ammoChanged;

	protected float bXPos;
	protected float bYPos;
	protected float bXSpeed;
	protected float bYSpeed;

	protected bool _slashDelay;
	private double _slashDelayCounter;
	public double SlashDelayMs = 1.000d;

	private const string ANIMATOR_PARAM = "anim";


	protected bool _paused;
	private enum Animations
	{
		Idle = 0,
		Running = 1,
		Jumping = 2,
		Melee = 3,
		Shooting = 4,
		Aiming = 5,
		Shot = 6,
		Stabbed = 7
	}

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

	protected virtual void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		_sRenderer = GetComponent<SpriteRenderer>();
		HealthSlider = GetComponentInChildren<Slider>();

		_slashCol = Instantiate(SwordSlash.gameObject,
			FacingRight
				? new Vector3(transform.position.x + SlashOffset, transform.position.y, transform.position.y)
				: new Vector3(transform.position.x - SlashOffset, transform.position.y, transform.position.y),
			transform.rotation) as GameObject;

		HealthSlider.maxValue = MaxHealth;
		HealthSlider.value = MaxHealth;

		GunLight.enabled = false;
		GunLight.intensity = 0;

		Ammo = StartingAmmo;
		Health = StartingHealth;

		AmmoText.enabled = false;
		SpawnCanvas.enabled = false;

		JumpCount = MaxJumpCount;
		_groundCheckCollider = GroundCheck.GetComponent<BoxCollider2D>();


		if (string.IsNullOrEmpty(Control) && Debug.isDebugBuild)
		{
			Control = "j1";
			this.Ammo = this.MaxAmmo = 99;
		}
	}

	private bool tempGrounded = true;

	private void SetGrounded()
	{
		var grounded = _groundCheckCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));

		Grounded = grounded;

		if (!tempGrounded && (grounded || _inFrontOfLadder))
		{
			JumpCount = MaxJumpCount;
		}

		tempGrounded = grounded;
	}

	protected virtual void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		AmmoText.text = Ammo.ToString();

		SetGrounded();

		UpdateSlashColPos();
		CheckTimers();


		if (Input.GetButtonDown(Control + "Fire") && !_paused && Ammo > 0)
		{
			_softAim = true;
		}

		if (Input.GetButtonDown(Control + "Jump") && (true || _inFrontOfLadder) && !_paused && !_aiming)
		{
			this.Jump();
		}

		if (Input.GetButtonUp(Control + "Fire") && !_paused && Ammo > 0 && !_hit && !_shooting)
		{
			if (_aimCanceled)
			{
				_aimCanceled = false;
				return;
			}
			CmdShoot();
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
	}

	protected void Slash()
	{
		_slashing = true;

		_slashCol.SendMessage("GetCol", _slashing);
		_audio.PlayOneShot(SlashClip);
	}

	protected void Jump()
	{
		if (AbleToJump)
		{
			_jumped = true;
			if (_inFrontOfLadder)
			{
				_inFrontOfLadder = false;
			}

			if (JumpCount == MaxJumpCount)
			{
				// First jump
				_rb2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
			}
			else
			{
				// Second jump
				_rb2D.AddForce(new Vector2(0, JumpForce * SecondJumpMultiplier), ForceMode2D.Impulse);
			}

			JumpCount--;
		}
	}

	protected void HandleAnimations()
	{
		var jumping = !Grounded;

		if (_hitByBullet)
		{
			// Shot
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Shot);
		}
		else if (_hitByMelee)
		{
			// Slashed
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Stabbed);
		}
		else if (_shooting)
		{
			// Shooting
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Shooting);
		}
		else if (_aiming)
		{
			// Aiming
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Aiming);
		}
		else if (_slashing)
		{
			// Melee
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Melee);
		}
		else if (jumping)
		{
			// Jumping
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Jumping);
		}
		else if (_horizontal != 0)
		{
			// Running
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Running);
		}
		else
		{
			// Idle
			_animator.SetInteger(ANIMATOR_PARAM, (int)Animations.Idle);
		}
	}


	protected virtual void FixedUpdate()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		if (_spawned)
		{
			return;
		}

		float h = Input.GetAxis(Control + "Horizontal");
		float v = Input.GetAxis(Control + "Vertical");

		// Stop movement if value is below movement lock
		if ((h < MovementLock && h > 0) || (h > -MovementLock && h < 0))
		{
			h = 0;
		}

		_horizontal = h;
		_vertical = v;

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

		if (_aiming)
		{
			_rb2D.velocity = new Vector2(0, _rb2D.velocity.y);
		}

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
			_rb2D.gravityScale = GravityScale;

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

		CheckFlip();

	}

	private void CheckFlip()
	{
		if (_horizontal > 0 && !FacingRight)
		{
			Flip();
		}
		else if (_horizontal < 0 && FacingRight)
		{
			Flip();
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		// Rope
		if (other.name.StartsWith("ropeAttached") && _up)
		{
			_inFrontOfLadder = true;
		}

		// Bullet
		if (other.name.StartsWith("Bullet"))
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
			// Teleport to the top 
			transform.position = new Vector3(transform.position.x, 6, transform.position.z);
		}

		// RL - LR Collider
		if (other.name.StartsWith("LRCollider"))
		{
			// Teleport to the top 
			transform.position = new Vector3(9, transform.position.y, transform.position.z);
		}

		else if (other.name.StartsWith("RLCollider"))
		{
			// Teleport to the top 
			transform.position = new Vector3(-9, transform.position.y, transform.position.z);
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

	protected void DrawLine()
	{
		AimLine.GetComponent<SpriteRenderer>().enabled = true;

		var line = AimLine;
		var bXPos = 0f;
		var bYPos = 0f;
		var bRotation = 0f;

		if (!_bulletRight && !_bulletLeft)
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
			bXPos = GunOffset;
			bRotation += 180;
		}
		else if (_bulletLeft)
		{
			bXPos = -GunOffset;
		}

		line.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);
		line.eulerAngles = new Vector3(0, 0, -bRotation);

	}

	protected virtual void LowerHealth(int damage)
	{
		VibrateGamePad();
		Health -= damage;
	}


	protected virtual void UpdateHealthSlider(int health)
	{
		HealthSlider.value = health;
	}

	protected virtual void GiveHealth(int health)
	{
		Health += health;

		if (Health > MaxHealth)
		{
			Health = MaxHealth;
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
		_slashCol.transform.position = FacingRight
			? new Vector3(transform.position.x + SlashOffset, transform.position.y, transform.position.z)
			: new Vector3(transform.position.x - SlashOffset, transform.position.y, transform.position.z);
	}

	private void CheckTimers()
	{
		CheckHealthBar();
		CheckSlashingTimer();
		CheckSlashDelay();
		CheckShootingTimer();
		CheckHitTimer();
		CheckGunLightTimer();
		CheckHealedTimer();
		CheckSpawnedTimer();
		CheckAimTimer();
		CheckVibrationTimer();
		CheckAmmoTimer();
		CheckJumpDelay();
	}

	private void CheckHealthBar()
	{
		if (_hit || _healed)
		{
			HealthSlider.GetComponentInParent<Canvas>().enabled = true;
		}
		else
		{
			HealthSlider.GetComponentInParent<Canvas>().enabled = false;
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
				_slashDelay = true;
				_slashCol.SendMessage("GetCol", _slashing);
				_slashingCounter = 0.000d;
			}
		}
	}

	private void CheckShootingTimer()
	{
		if (_shooting)
		{
			_shootingCounter += 1 * Time.deltaTime;
			if (_shootingCounter >= ShootingMs)
			{
				_shooting = false;
				_shootingCounter = 0.000d;
			}
		}
	}
	private void CheckSlashDelay()
	{
		if (_slashDelay)
		{
			_slashDelayCounter += 1 * Time.deltaTime;
			if (_slashDelayCounter >= SlashDelayMs)
			{
				_slashDelayCounter = 0.000d;
				_slashDelay = false;
			}
		}
	}

	private void CheckJumpDelay()
	{
		if (_jumped)
		{
			jumpDelayCounter += 1 * Time.deltaTime;

			if (jumpDelayCounter >= JumpDelayMs)
			{
				jumpDelayCounter = 0.000d;
				_jumped = false;
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
				_hitByMelee = false;
				_hitByBullet = false;
				_hitCounter = 0.000d;
				_flashTimer = 0.000d;
				_sRenderer.enabled = true;
			}
		}
	}

	private void CheckSpawnedTimer()
	{
		if (_spawned)
		{
			_spawnedCounter += 1 * Time.deltaTime;

			if (_spawnedCounter >= _spawnedMs)
			{
				SpawnCanvas.enabled = false;
				_spawned = false;
				_spawnedCounter = 0.000d;
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


	private void CheckAmmoTimer()
	{
		if (_ammoChanged)
		{
			_ammoCounter += 1 * Time.deltaTime;

			if (_ammoCounter >= AmmoMs)
			{
				_ammoChanged = false;
				_ammoCounter = 0.000d;
				AmmoText.enabled = false;
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

	public virtual void Flip()
	{
		FlipSliderLeftRight(HealthSlider);

		FacingRight = !FacingRight;

		FlipLeftRight(transform);
		FlipLeftRight(AmmoText.transform);
	}

	private void FlipSliderLeftRight(Slider slider)
	{
		slider.direction = FacingRight ? Slider.Direction.RightToLeft : Slider.Direction.LeftToRight;
	}

	protected void FlipLeftRight(Transform obj)
	{
		var scale = obj.localScale;
		scale.x *= -1;

		obj.localScale = scale;
	}

	[Command]
	protected virtual void CmdShoot()
	{
		_shooting = true;
		if (!_bulletRight && !_bulletLeft)
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
			bXPos = GunOffset;
		}
		else if (_bulletLeft)
		{
			bXSpeed = -BulletSpeed;
			bXPos = -GunOffset;
		}

		var shotTransform = Instantiate(Bullet) as Transform;
		shotTransform.position = new Vector3(transform.position.x + bXPos, transform.position.y + bYPos, transform.position.z);

		shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bXSpeed, bYSpeed);

		NetworkServer.Spawn(shotTransform.gameObject);

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
	}


	protected void VibrateGamePad()
	{
		switch (Control)
		{
			case "j1":
				GamePad.SetVibration(PlayerIndex.One, 1f, 1f);
				break;
			case "j2":
				GamePad.SetVibration(PlayerIndex.Two, 1f, 1f);
				break;
			case "j3":
				GamePad.SetVibration(PlayerIndex.Three, 1f, 1f);
				break;
			case "j4":
				GamePad.SetVibration(PlayerIndex.Four, 1f, 1f);
				break;
			default:
				global.LogDebug(Log, "Keyboard, cannot vibrate");
				break;
		}

		vibrating = true;
	}

	void CheckVibrationTimer()
	{
		if (vibrating)
		{
			vibrationCounter += 1 * Time.deltaTime;

			if (vibrationCounter >= VibrationMs)
			{
				vibrationCounter = 0.000d;

				switch (Control)
				{
					case "j1":
						GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
						break;
					case "j2":
						GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
						break;
					case "j3":
						GamePad.SetVibration(PlayerIndex.Three, 0f, 0f);
						break;
					case "j4":
						GamePad.SetVibration(PlayerIndex.Four, 0f, 0f);
						break;
				}

				vibrating = false;
			}
		}
	}


	protected void CheckHealth(Collider2D other)
	{
		if (Health <= 0)
		{
			Spawn--;

			if (Spawn > 0)
			{
				_hit = true;
				Instantiate(BloodSplatter, transform.position, transform.rotation);
				Health = StartingHealth;
				HealthSlider.value = Health;
				Ammo = StartingAmmo;

				SpawnCanvas.enabled = true;

				switch (Spawn)
				{
					case 3:
						SpawnImage.sprite = HealthThree;
						break;
					case 2:
						SpawnImage.sprite = HealthTwo;
						break;
					case 1:
						SpawnImage.sprite = HealthOne;
						break;
				}
				_spawned = true;

				if (other.name.StartsWith("Bullet", StringComparison.InvariantCultureIgnoreCase))
				{
					_hitByBullet = true;
				}
				else if (other.name.StartsWith("slash", StringComparison.InvariantCultureIgnoreCase))
				{
					_hitByMelee = true;
				}
			}
			else if (Spawn <= 0)
			{
				Die(other);
			}
		}
	}

	protected void Die(Collider2D other)
	{
		_dead = true;
		Instantiate(BloodSplatter, transform.position, transform.rotation);

		if (!other.name.Equals("FallCollider"))
		{
			var deadPlayer = Instantiate(DeadPlayer.gameObject, transform.position, transform.rotation) as GameObject;
		}
		Destroy(gameObject);
	}

}
