using System;
using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;

    [HideInInspector]
    public bool jump = false;

    [HideInInspector]
    public bool run = false;

    public float moveForce = 365f;
    public float MAX_SPEED = 5f;
    public float jumpForce = 10f;
    public float bulletSpeed = 20f;
    public Transform groundCheck;
    public Transform groundCheck2;
    public Transform bullet;

    private bool grounded = false;
    private bool grounded2 = false;
    private bool inFrontOfLadder = false;
    private bool up = false;
    private Rigidbody2D rb2d;

    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        if (Input.GetButtonDown("Jump") && ((grounded || grounded2) || inFrontOfLadder))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetAxis("Vertical") > 0.3f)
        {
            up = true;
        }
        else
        {
            up = false;
        }

    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        if (h == 0)
        {
            animator.SetInteger("anim",0);
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

        shotTransform.position = transform.position;

        if (facingRight)
        {
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        }
        else
        {
            shotTransform.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("ropeAttached") && up)
        {
            inFrontOfLadder = true;
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
