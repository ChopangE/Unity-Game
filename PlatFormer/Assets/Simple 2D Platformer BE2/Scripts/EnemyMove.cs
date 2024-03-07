using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    int nextMove;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Think();
    }

    void FixedUpdate() {
        
        rb.velocity = new Vector2(nextMove, rb.velocity.y);
        Vector2 frontVec = new Vector2(nextMove * 0.4f + rb.position.x, rb.position.y);
        Debug.DrawRay(frontVec, Vector2.down, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Ground"));
        if(hit.collider == null) {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 5f);
        }

    }

    void Think() {
        nextMove = Random.Range(-1, 2);
        sprite.flipX = nextMove >= 0 ? true : false;

        Invoke("Think", 5f);
    }
}
