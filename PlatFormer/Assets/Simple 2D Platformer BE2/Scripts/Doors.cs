using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    public Transform connectDoor;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            other.transform.position = connectDoor.position + new Vector3(0,2,0);
        }
    }
}
