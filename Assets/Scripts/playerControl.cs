using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;

    [HideInInspector]
    public bool jump = false;

    [HideInInspector]
    public bool run = false;

    [HideInInspector]
    public string control;

    public float moveForce = 365f;
    public float MAX_SPEED = 5f;
    public float jumpForce = 10f;
    public float bulletSpeed = 20f;
    public Transform groundCheck;
    public Transform groundCheck2;
    public Transform bullet;
    public Transform healthBar;
    public Transform explosion;
    public AudioClip gunShot;
    public int health = 3;
    public int ammo = 3;

    private double counter = 0.000d;
    private bool grounded = false;
    private bool grounded2 = false;
    private bool inFrontOfLadder = false;
    private bool up = false;
    private Rigidbody2D rb2d;
    private AudioSource audio;
    private Animator animator;
    private SpriteRenderer healthBarRender;
    private bool dead = false;

    private Sprite health1;
    private Sprite health2;
    private Sprite health3;

    private string playerClass;
    private int playerNum;

    // Use this for initialization
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        health1 = Resources.Load<Sprite>("threehealth");
        health2 = Resources.Load<Sprite>("quarterhealth");
        health3 = Resources.Load<Sprite>("nohealth");

        healthBarRender = healthBar.GetComponent<SpriteRenderer>();
        healthBarRender.enabled = false;

        if (playerSelect.playerCount == 0)
        {
            control = "j1";
        }
        else
        {
            control = playerSelect.playerList[playerSelect.playerCount - 1].Control;
            playerClass = playerSelect.playerList[playerSelect.playerCount - 1].Class;
            playerNum = playerSelect.playerList[playerSelect.playerCount - 1].Num;
            playerSelect.playerCount--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        grounded2 = Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }

        if (Input.GetButtonDown(control + "Jump") && ((grounded || grounded2) || inFrontOfLadder))
        {
            jump = true;
        }

        if (Input.GetButtonDown(control + "Fire"))
        {
            Shoot();
        }

        if (Input.GetAxis(control + "Vertical") > 0.3f)
        {
            up = true;
        }
        else
        {
            up = false;
        }

        CheckHealthBar();

    }


    private void CheckHealthBar()
    {
        if (healthBarRender.enabled)
        {
            counter += 1 * Time.deltaTime;

            if (counter >= 2.000d)
            {
                healthBarRender.enabled = false;
                counter = 0.000d;
            }
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis(control + "Horizontal");

        float v = Input.GetAxis(control + "Vertical");

        if (h == 0)
        {
            animator.SetInteger("anim", 0);
        }

        if (inFrontOfLadder)
        {
            rb2d.gravityScale = 0;
            rb2d.velocity = Vector3.zero;


            if (v * rb2d.velocity.y < MAX_SPEED)
            {
                rb2d.AddForce(Vector2.up * v * moveForce);
            }

            if (Mathf.Abs(rb2d.velocity.y) > MAX_SPEED)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Sign(rb2d.velocity.y) * MAX_SPEED);
            }

        }
        else
        {
            rb2d.gravityScale = 4;

            if (h * rb2d.velocity.x < MAX_SPEED)
            {
                rb2d.AddForce(Vector2.right * h * moveForce);
            }

            if (Mathf.Abs(rb2d.velocity.x) > MAX_SPEED)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * MAX_SPEED, rb2d.velocity.y);
            }

            if (h != 0)
            {
                animator.SetInteger("anim", 1);
            }
        }

        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }

        if (jump)
        {
            if (inFrontOfLadder)
            {
                inFrontOfLadder = false;
            }
            rb2d.AddForce(new Vector2(0, jumpForce));
            jump = false;

        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        transform.localScale = theScale;


    }

    void Respawn()
    {
        transform.position = new Vector3(-5, 3, transform.position.z);
    }

    void Shoot()
    {
        var shotTransform = Instantiate(bullet) as Transform;

        if (facingRight)
        {
            shotTransform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        }
        else
        {
            shotTransform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);

        }
        audio.PlayOneShot(gunShot);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached") && up)
        {
            inFrontOfLadder = true;
        }

        if (other.name.Equals("Bullet(Clone)"))
        {
            health--;
            if (health == 0)
            {
                // Death
                dead = true;
                localController.AlivePlayers.Remove(localController.AlivePlayers.Find(p => p.Num == playerNum));
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }

            switch (health)
            {
                case 2:
                    healthBarRender.sprite = health1;
                    healthBarRender.enabled = true;
                    break;
                case 1:
                    healthBarRender.sprite = health2;
                    healthBarRender.enabled = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached"))
        {
            inFrontOfLadder = false;
            up = false;
        }
    }

}
