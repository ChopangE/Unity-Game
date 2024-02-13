using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 1f;

    private bool isTrigger = false;

    public bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isTrigger == false && isStart == true)
        {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Target")
        {
            GameObject child = transform.GetChild(0).gameObject;
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            childSprite.enabled = true;

            transform.SetParent(collision.gameObject.transform);
            isTrigger = true;
        }
    }

    public void Launch()
    {
        isStart = true;

    }
}
