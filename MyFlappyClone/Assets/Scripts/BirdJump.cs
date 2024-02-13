using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdJump : MonoBehaviour
{
    [SerializeField]
    private float JumpPower = 3.5f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.velocity = Vector2.up * JumpPower;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (SCore.BestScore < SCore.Score) SCore.BestScore = SCore.Score;
        SceneManager.LoadScene("GameOver");
    }
}
