using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("-----------[Core]")]
    public int score;
    public int maxLevel;
    public bool isOver;

    [Header("-----------[Object Pooling]")]
    public GameObject donglePrefab;
    public Transform dongleSpawner;
    public List<Dongle> donglePool;
    public GameObject effectPrefab;
    public Transform effectSpawner;
    public List<ParticleSystem> effectPool;
    [Range(1,30)]
    public int poolSize;
    public int poolCursor;
    public Dongle lastDongle;

    [Header("-----------[Audio]")]
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayers;
    public AudioClip[] sfxCilps;
    public enum Sfx { LevelUp, Next = 3, Attach, Button, Over};         // like static
    int sfxPtr;
    
    public static GameManager instance;

    [Header("-----------[UI]")]
    public GameObject startGroup;
    public GameObject endGroup;
    public Text scoreText;
    public Text bestText;
    public Text subScoreText;

    [Header("-----------[ ETC ]")]
    public GameObject line;
    public GameObject bottom;

    void Awake() {
        instance = this;
        Application.targetFrameRate = 60;

        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();
        for(int index = 0; index< poolSize; index++) {
            MakeDongle();
        }

        if (PlayerPrefs.HasKey("MaxScore")) {
            PlayerPrefs.SetInt("MaxScore", 0);
        }

        bestText.text = "Best : " + PlayerPrefs.GetInt("MaxScore").ToString();
    }
    public void GameStart()
    {
        //object active
        line.SetActive(true);
        bottom.SetActive(true);
        scoreText.gameObject.SetActive(true);
        bestText.gameObject.SetActive(true);
        startGroup.SetActive(false);

        bgmPlayer.Play();
        SfxPlay(Sfx.Button);

        Invoke("NextDongle", 1.5f);
    }
    
    Dongle MakeDongle() {
        //Effect
        GameObject instantEffectObj = Instantiate(effectPrefab, effectSpawner);
        instantEffectObj.name = "Effect " + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);

        //Dongle
        GameObject instantDongleObj = Instantiate(donglePrefab, dongleSpawner);
        instantDongleObj.name = "Dongle " + donglePool.Count;
        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>();
        instantDongle.effect = instantEffect;
        donglePool.Add(instantDongle);

        return instantDongle;
    }


    Dongle GetDongle() {
        for(int index = 0; index < donglePool.Count; index++) {             //pool에 있으면 가져옴.
            poolCursor = (poolCursor + 1) % donglePool.Count;
            if (!donglePool[poolCursor].gameObject.activeSelf) {
                return donglePool[poolCursor];
            }
        }
        return MakeDongle();                                            //새로만듦.
    }
    void NextDongle() {
        if(isOver) return;

        lastDongle = GetDongle();
        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);

        SfxPlay(Sfx.Next);
        StartCoroutine("WaitExit");
    }

    IEnumerator WaitExit() {

        while(lastDongle != null) {
            yield return null;      //그냥 무한루프 쓰면 unity가 멈춘다.(너무 많은 연산량 때문?)
        }

        yield return new WaitForSeconds(2f);

        NextDongle();
    }
    public void TouchDown() {
        if (lastDongle == null) return;
        lastDongle.Drag();
    }

    public void TouchUp() {
        if (lastDongle == null) return;
        lastDongle.Drop();
        lastDongle = null;
    }

    public void GameOver() {

        if (isOver) {
            return;
        }

        isOver = true;

        StartCoroutine(GameOverRoutine());

    }
    

    IEnumerator GameOverRoutine() {


        // 1. Check all active dongles in Scene
        Dongle[] dongles = FindObjectsOfType<Dongle>();
        
        //2. Before delete, all dongle's rg are unactive
        
        for(int index = 0; index < dongles.Length; index++) {
            dongles[index].rg.simulated = false;
        }

        //3. Delete dongles in #1 list
        for (int index = 0; index < dongles.Length; index++) {
            dongles[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        int bestScore = Mathf.Max(PlayerPrefs.GetInt("MaxScore"), score);
        PlayerPrefs.SetInt("MaxScore", bestScore);

        subScoreText.text = "점수 : " + score.ToString(); 
        endGroup.SetActive(true);

        bgmPlayer.Stop();
        SfxPlay(Sfx.Over);
    }

    public void Reset() {

        SfxPlay(Sfx.Button);
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine() {

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }


    public void SfxPlay(Sfx type) {

        switch (type) {
            case Sfx.LevelUp:
                sfxPlayers[sfxPtr].clip = sfxCilps[Random.Range(0,3)];
                break;
            case Sfx.Next:
                sfxPlayers[sfxPtr].clip = sfxCilps[(int)Sfx.Next];
                break;
            case Sfx.Attach:
                sfxPlayers[sfxPtr].clip = sfxCilps[(int)Sfx.Attach];
                break;
            case Sfx.Button:
                sfxPlayers[sfxPtr].clip = sfxCilps[(int)Sfx.Button];
                break;
            case Sfx.Over:
                sfxPlayers[sfxPtr].clip = sfxCilps[(int)Sfx.Over];
                break;
        }

        sfxPlayers[sfxPtr].Play();
        sfxPtr =(sfxPtr +1 ) % sfxPlayers.Length;
    }

    void Update() {

        if (Input.GetButtonDown("Cancel")) {
            Application.Quit();
        }
    }

    void LateUpdate() {
        scoreText.text = "Score : " + score.ToString();
    }

}
