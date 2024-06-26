using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake() {
        player = GameManager.instance.player;
    }
    void Update() {
        if (!GameManager.instance.isLive) return;
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(float damamge, int count) {
        this.damage = damamge * Character.Damage;
        this.count += count;

        if (id == 0)
            Batch();


        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data) {

        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for(int index = 0; index < GameManager.instance.pool.prefabs.Length; index++) {
            if(data.projectTile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }


        switch (id) {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    
    void Batch() {
        for(int index = 0; index < count; index++) {
            Transform bullet;
            if(index < transform.childCount) {
                bullet = transform.GetChild(index);                                 //원래 돌고 있던 삽하나 가져오기
            }
            else {
                bullet= GameManager.instance.pool.Get(prefabId).transform;          //pool에서 새로운 삽하나 할당받기
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(Vector3.up * 1.5f, Space.Self);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);  //-1 is Infinity Per.
        }
    }
    void Fire() {

        if (!player.scanner.nearestTarget) return;
        
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos-transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;          
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count , dir);  //-1 is Infinity Per.

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

    }
}
