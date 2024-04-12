using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLeyer;
    private bool isFacingRight;
    private float horizintal;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizintal * moveSpeed, rb.velocity.y);
    }

    private void Update()
    {
        horizintal = Input.GetAxis("Horizontal");
        Flip();
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if(Input.GetButtonDown("Jump") && rb.velocity.y>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Flip()
    {
        if(isFacingRight && horizintal<0 || !isFacingRight && horizintal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLeyer);    }


}
