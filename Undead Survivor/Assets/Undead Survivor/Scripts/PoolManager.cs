using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    void Awake() {
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++) {
            pools[index] = new List<GameObject> ();
        }
    }

    public GameObject Get(int index) {
        
        GameObject select = null;
        
        
        foreach(GameObject item in pools[index]) {
            if (!item.activeSelf) {                             //list안에 비활성화된 객체(놀고있는 객체)있으면 재활용
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(select == null) {                                        //list안에 비활성화된(놀고있는) 객체 없다면 새로 만듦
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
