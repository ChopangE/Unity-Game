using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public ParticleSystem effect;
    public int level;
    public bool isDrag;
    public bool isMerge;
    public bool isAttach;

    public Rigidbody2D rg;
    CircleCollider2D circle;
    Animator anim;
    SpriteRenderer sprite;

    float deadTime;

    void Awake() {
        
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circle = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    
    void OnEnable() {

        anim.SetInteger("Level", level);

    }

    void OnDisable() {

        level = 0;
        isDrag = false;
        isMerge = false;
        isAttach = false;
        
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        rg.simulated = false;
        rg.velocity = Vector2.zero;
        rg.angularVelocity = 0;
        circle.enabled = true;
    }

    void Update()
    {
        if (!isDrag) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //x축 경계 설정
        float leftBorder = -4.2f + transform.localScale.x / 2;
        float rightBorder = 4.2f - transform.localScale.x / 2;

        if(mousePos.x < leftBorder) {
            mousePos.x = leftBorder;
        }
        else if(mousePos.x > rightBorder) {
            mousePos.x = rightBorder; 
        }
        mousePos.y = 8;
        mousePos.z = 0;
        transform.position = Vector3.Lerp(transform.position,mousePos, 0.1f);
    }

    public void Drag() {

        isDrag = true;
    }
    public void Drop() {

        isDrag = false;
        rg.simulated = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {

        StartCoroutine("AttachRoutine");
    }

    IEnumerator AttachRoutine() {

        if (isAttach) {                     //While sound is playing, another sound playing is blocked.
            yield break;
        }

        isAttach = true;
        GameManager.instance.SfxPlay(GameManager.Sfx.Attach);

        yield return new WaitForSeconds(0.2f);

        isAttach = false;
    }


    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Dongle") {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && !other.isMerge && level < 7) {
                float meX = transform.position.x;
                float meY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;
                if(meY < otherY || (meY == otherY && meX > otherX)) {

                    other.Hide(transform.position);

                    LevelUp();
                }

            }
        }
    }
    public void Hide(Vector3 targetPos) {

        isMerge = true;

        rg.simulated = false;
        circle.enabled = false;

        if(targetPos == Vector3.up * 100) {
            EffectPlay();
        }

        StartCoroutine(HideRoutine(targetPos));
    }

    IEnumerator HideRoutine(Vector3 targetPos) {

        int frameCount = 0;

        while(frameCount < 20) {
            frameCount++;
            if (targetPos != Vector3.up * 100) {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            else if (targetPos == Vector3.up * 100) {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }
            yield return null;
        }

        GameManager.instance.score += (int)Mathf.Pow(2, level);
        isMerge = false;
        gameObject.SetActive(false);

    }

    void LevelUp() {
        isMerge = true;

        rg.velocity = Vector2.zero;
        rg.angularVelocity = 0f;

        StartCoroutine(LevelUpRoutine());

    }

    IEnumerator LevelUpRoutine() {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1);
        EffectPlay();
        GameManager.instance.SfxPlay(GameManager.Sfx.LevelUp);
        yield return new WaitForSeconds(0.3f);
        level++;

        GameManager.instance.maxLevel = Mathf.Max(level, GameManager.instance.maxLevel);

        isMerge = false;
    }

    void OnTriggerStay2D(Collider2D collision) {
        
        if(collision.tag == "Finish") {
            deadTime += Time.deltaTime;
            if (deadTime > 2) {
                sprite.color = Color.red;
            }
            if(deadTime > 5) {
                //GameOver
                GameManager.instance.GameOver();
            }

        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        
        if(collision.tag == "Finish") {
            deadTime = 0;
            sprite.color = Color.white;         //Default color
        }
    }

    void EffectPlay() {

        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;     //According to level, its scale matches.
        effect.Play();
    }
}
