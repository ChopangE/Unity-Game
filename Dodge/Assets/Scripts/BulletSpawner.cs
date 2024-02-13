using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;
    public int direct = 0;
    public GameObject player;
    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;
    private float moveSpeed = 4f;
    private float moveRange= 0f;
    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = player.transform;
        moveSpeed = Random.Range(1f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        moveRange += Time.deltaTime * moveSpeed ;
        if(moveRange > 7f || moveRange < -7f)
        {
            moveSpeed = -1 * moveSpeed;
        }
        if(direct == 0)
        {
            transform.position = new Vector3(transform.position.x, 1f, moveRange); 
        }
        else
        {
            transform.position = new Vector3(moveRange, 1f, transform.position.z);
        }
        timeAfterSpawn += Time.deltaTime;
        if(timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
