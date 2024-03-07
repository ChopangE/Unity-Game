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
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update() {

        if (Input.GetButtonDown("Jump")) {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (Input.GetButtonUp("Horizontal")) {
            rb.velocity = Vector2.zero;
                //new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
        }

        
        if (Input.GetButton("Horizontal")) {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        if(Mathf.Abs(rb.velocity.x) < 0.4f) {
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
}
