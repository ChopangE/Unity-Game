using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRigidBody;
    public float speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        //InputVec = new Vector3(xInput, 0 , zInput).normalized * speed * Time.deltaTime;
        
        //playerRigidBody.MovePosition(playerRigidBody.position + InputVec);
        float xSpeed = speed * xInput;
        float zSpeed = speed * zInput;
        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidBody.velocity = newVelocity;

    }

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EndGame();
    }
}
