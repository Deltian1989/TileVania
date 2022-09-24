using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Player>())
            transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
}
