using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePipe : MonoBehaviour
{
    public GameObject pipe;

    private float time = 0f;


    [SerializeField]
    private float timeInterval = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > timeInterval)
        {
            GameObject newPipe = Instantiate(pipe);
            newPipe.transform.position = new Vector3(3f, Random.Range(-1.5f,5f), 0);
            time = 0f;
            Destroy(newPipe, 5f);
        }
        
    }

}
