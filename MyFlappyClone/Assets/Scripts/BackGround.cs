using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * moveSpeed*Time.deltaTime;
        if(transform.position.x < -5.6)
        {
            transform.position = new Vector3(5.6f, 0, 0);
        }
    }
}
