using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Score : " + SCore.Score.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
