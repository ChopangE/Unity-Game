using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator anim;
    public int jumpCount;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update() {

        
        
        if (Input.GetButtonDown("Jump") && jumpCount < 2) {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
        }
        else if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
            rb.velocity = rb.velocity * 0.5f;
        }

        if (Input.GetButtonUp("Horizontal") && jumpCount == 0) {
            rb.velocity = Vector2.zero;
        }

        
        if (Input.GetButton("Horizontal")) {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        if(Mathf.Abs(rb.velocity.x) < 0.3f) {
            anim.SetBool("Run", false);
        }
        else {
            anim.SetBool("Run", true);

        }
    }

    void FixedUpdate() {
        float x = Input.GetAxisRaw("Horizontal");

        rb.AddForce(Vector2.right * x, ForceMode2D.Impulse);

        if(rb.velocity.x > maxSpeed) {
            rb.velocity = new Vector2 (maxSpeed, rb.velocity.y);
        }
        else if(rb.velocity.x < -maxSpeed) {
            rb.velocity = new Vector2 (-maxSpeed, rb.velocity.y);
        }
       
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.tag);
        
        anim.SetBool("Jump", false);
        jumpCount = 0;
        rb.velocity = Vector2.zero;
        
    }
    void OnCollisionExit2D(Collision2D collision) {
        anim.SetBool("Jump", true);
    }
}
