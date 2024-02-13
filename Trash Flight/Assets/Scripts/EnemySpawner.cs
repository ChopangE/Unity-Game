using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private GameObject boss;

    private float[] arrPosX = {-2.2f, -1.1f, 0f, 1.1f, 2.2f};
    
    [SerializeField]
    private float spawnInterval = 1.5f;
    // Start is called before the first frame update

    void Start()
    {
       StartEnemyRoutine();
    }
    public void StopEnemyRoutine(){
        StopCoroutine("EnemyRoutine");
    }
    void StartEnemyRoutine(){
        StartCoroutine("EnemyRoutine");
    }

    IEnumerator EnemyRoutine(){
        yield return new WaitForSeconds(3f);    //3초 대기

        float moveSpeed = 5f;
        int spawnCount = 0;
        int enemyIndex = 0;

        while(true){
            foreach(float posx in arrPosX){
                SpawnEnemy(posx, enemyIndex, moveSpeed);
            }
            
            spawnCount++;
            if(spawnCount % 10 == 0){ // 10, 20
                enemyIndex++;
                moveSpeed += 2;
            }
            
            if(enemyIndex >= enemies.Length){
                SpawnBoss();
                enemyIndex = 0;
                moveSpeed = 5f;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(float posX, int index, float moveSpeed){
        Vector3 spawnPos = new Vector3(posX, transform.position.y, transform.position.z);
        if(Random.Range(0,5) == 0){ //20%확률로 +1Level trash 등장
            index++;
        }

        if(index >= enemies.Length){
            index = enemies.Length - 1;
        }
        
        GameObject enemyObject = Instantiate(enemies[index], spawnPos, Quaternion.identity); 
        Enemy enemy = enemyObject.GetComponent<Enemy>(); 
        enemy.setMoveSpeed(moveSpeed);
    }

    void SpawnBoss(){
        Instantiate(boss, transform.position, Quaternion.identity);
    }
}
