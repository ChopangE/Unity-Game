using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinStarter : MonoBehaviour
{
    [SerializeField]
    private GameObject pinObject;

    private Pin currPin;
    // Start is called before the first frame update
    void Start()
    {
        PreparePin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currPin.Launch();
            currPin = null;
            Invoke("PreparePin", 0.1f);
        }    
    }
    void PreparePin()
    {
        GameObject pin = Instantiate(pinObject, transform.position, Quaternion.identity);
        currPin = pin.GetComponent<Pin>();
    }
}
