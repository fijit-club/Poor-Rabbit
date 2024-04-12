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
        horizintal = Input.GetAxis("Horizontal");
        if (SimpleInput.GetButton("Right") || horizintal > 0)
        {
            rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);
        }
        else if (SimpleInput.GetButton("Left") || horizintal < 0)
        {
            rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Update()
    {
        horizintal = Input.GetAxis("Horizontal");
        Flip();
        if (IsGrounded() && (Input.GetButtonDown("Jump") || SimpleInput.GetButtonDown("Jump")))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if((Input.GetButtonDown("Jump") || SimpleInput.GetButtonDown("Jump")) && rb.velocity.y>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.SpawnCarrots();
        }
    }

    private void Flip()
    {
        if(isFacingRight && (horizintal<0 || SimpleInput.GetButton("Right")) || !isFacingRight && (horizintal > 0 || SimpleInput.GetButton("Left")))
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
