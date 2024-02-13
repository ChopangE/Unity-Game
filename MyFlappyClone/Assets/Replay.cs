using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    // Start is called before the first frame update
    public void RePlay()
    {
        SCore.Score = 0;
        SceneManager.LoadScene("SampleScene");
    }
}
