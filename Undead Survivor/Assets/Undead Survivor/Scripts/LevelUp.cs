using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    public Follow follow;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }
    public void Show() {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
        follow.gameObject.transform.localScale = Vector3.zero;
    }

    public void Hide() {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
        follow.gameObject.transform.localScale = Vector3.one;
    }

    public void Select(int index) {
        items[index].OnClick();
    }

    void Next() {
        //1. 모든 아이템 비활성화
        foreach(Item item in items) {
            item.gameObject.SetActive(false);
        }
        //2. 랜덤 3개 활성화
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0]!= ran[2])
                break;
        }

        for(int index = 0; index< ran.Length; index++) {
            Item ranItem = items[ran[index]];

            if(ranItem.level == ranItem.data.damages.Length) {
                items[4].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
            
        }
        //3. 만렙 아이템의 경우 포션으로 대체
    }
}
