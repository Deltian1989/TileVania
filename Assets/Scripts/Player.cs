using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] float deathForce = 30;

    bool isAlive = true;

    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Run();

            FlipSprite();

            Jump();

            ClimbLadder();

            CheckObstaclesTouching();
        }
    }

    void Run()
    {
        float controlThrow = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(controlThrow * speed, rb.velocity.y);

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        animator.SetBool("running", playerHasHorizontalSpeed);
    }

    private void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("climbing", false);

            rb.gravityScale = gravityScaleAtStart;

            return;
        }

        float controlThrow = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(rb.velocity.x, controlThrow * climbSpeed);

        rb.gravityScale = 0;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

        if (animator.GetBool("climbing"))
        {
            if (!playerHasHorizontalSpeed)
            {
                animator.StopPlayback();
            }
            else
            {
                animator.Play("climbing");
            }
        }
        else
        {
            animator.SetBool("climbing", playerHasHorizontalSpeed);
        }
        
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1);
        }
    }

    private void CheckObstaclesTouching()
    {
        if (isAlive)
        {
            if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Spikes")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("die");
        rb.AddForce(new Vector2(0, deathForce), ForceMode2D.Impulse);

        StartCoroutine(ProcessPlayerDeath());
    }

    private IEnumerator ProcessPlayerDeath()
    {
        yield return new WaitForSecondsRealtime(2);

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.GetComponent<EnemyMovement>())
        {
            if (isAlive)
            {
                Die();
            }
        }
    }
}
