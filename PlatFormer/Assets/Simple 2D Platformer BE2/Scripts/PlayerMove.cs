using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator anim;
    Collider2D coll;
    public int jumpCount;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    void Update() {
        

        if (Input.GetButtonDown("Jump") && jumpCount < 2) {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            anim.SetBool("Jump", true);

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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag != "Ground") return;
        if(Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Z))
        {
            jumpCount = 2;
            coll.isTrigger = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ground"))
        {
            
            coll.isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadZone"))
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.y > 0.6f) {
            anim.SetBool("Jump", false);
            if(jumpCount > 0) rb.velocity = Vector2.zero;

            jumpCount = 0;
            Debug.Log("Ground");

        }
        else {
            Debug.Log("Ceil");
            rb.velocity = Vector2.zero;
        }

    }
    void OnCollisionExit2D(Collision2D collision) {
        anim.SetBool("Jump", true);

    }
}
