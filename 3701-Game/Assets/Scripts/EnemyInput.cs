using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EnemyInput : MonoBehaviour
{
    public State beatState;
    private float timeInterval;
    [SerializeField]
    private MusicManager musicManager;

    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle, strike, death;
    [SerializeField]
    private GameObject highAttack, medAttack, lowAttack;



    private SpriteRenderer enemySprite;
    private State tempState;
    private Color originalColor;
    [SerializeField]
    private Color high, medium, low;

    public ButtonIndicator btnIndicator;
    [SerializeField]
    private Slider windupSlider;

    public Transform attackPos, defendPos;
    public GameObject outline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        tempState = beatState;
        originalColor = enemySprite.color;
    }

    // void Update()
    // {
    //     beatState = musicManager.beatStance;
    //     timeInterval = musicManager.timeInterval;
    //     if (tempState != beatState && beatState != State.Idle)
    //     {
    //         switch (beatState)
    //         {
    //             case State.ParryHigh:
    //                 StartCoroutine(Attack(State.ParryHigh, highParry, lowParry, high, 60 / musicManager.metroTempo * 7));
    //                 break;
    //             case State.ParryMedium:
    //                 StartCoroutine(Attack(State.ParryMedium, medParry, lowParry, medium, 60 / musicManager.metroTempo * 7));
    //                 break;
    //             case State.ParryLow:
    //                 StartCoroutine(Attack(State.ParryLow, lowParry, highParry, low, 60 / musicManager.metroTempo * 7));
    //                 break;
    //             default:
    //                 break;
    //         }
    //         //CheckBeatMap();
    //         tempState = beatState;
    //     }
    // }

    public void StartAttack(State state, float beats)
    {
        
        beatState = musicManager.beatStance;
        timeInterval = musicManager.timeInterval;
        switch (state)
        {
            case State.ParryHigh:
                CancelAttacks();
                StartCoroutine(Attack(State.ParryHigh, highParry, strike, highAttack, high, 60 / musicManager.metroTempo * beats));
                break;
            case State.ParryMedium:
                CancelAttacks();
                StartCoroutine(Attack(State.ParryMedium, medParry, strike, medAttack, medium, 60 / musicManager.metroTempo * beats));
                break;
            case State.ParryLow:
                CancelAttacks();
                StartCoroutine(Attack(State.ParryLow, lowParry, strike, lowAttack, low, 60 / musicManager.metroTempo * beats));
                break;
            case State.Hurting:
                EnemyDie();
                break;
            default:
                break;
        }
    }

    void CancelAttacks()
    {
        StopAllCoroutines();
        windupSlider.value = 0;
        windupSlider.gameObject.SetActive(false);
        btnIndicator.HideKey();
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;
    }

    private IEnumerator Attack(State state, Sprite startStance, Sprite endStance, GameObject followThrough, Color color, float outBeat)
    {
        btnIndicator.ShowKey(state);
        //enemySprite.color = color;
        enemySprite.sprite = startStance;
        //windupSlider.gameObject.SetActive(true);
        outline.SetActive(true);
        outline.GetComponent<SpriteRenderer>().sprite = enemySprite.sprite;
        outline.GetComponent<SpriteRenderer>().color = color;
        outline.transform.position = Camera.main.transform.position + (Camera.main.transform.position - transform.position);
        
        for(float i = 0; i < outBeat; i += Time.deltaTime)
        {
            windupSlider.value = i / outBeat;
            if((outline.transform.position - transform.position).magnitude > 0.1f)
                outline.transform.position = Camera.main.transform.position + ((Camera.main.transform.position - transform.position) * 0.8f) + (i / outBeat * 2 * (transform.position - Camera.main.transform.position));
            yield return null;
        }

        outline.SetActive(false);
        GameObject.Find("Judge").GetComponent<Judge>().Evaluate(state);
        windupSlider.gameObject.SetActive(false);
        enemySprite.sprite = endStance;
        btnIndicator.HideKey();
        transform.position = attackPos.position;
        followThrough.SetActive(true);
       
        yield return new WaitForSeconds(0.2f);
        transform.position = defendPos.position;
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;
        followThrough.SetActive(false);
    }

  
    private void EnemyDie()
    {
        enemySprite.sprite = death;
    }

    // public void CheckBeatMap()
    // {
        
    //     switch (beatState)
    //     {
    //         case State.ParryHigh:
    //             StartCoroutine(Attack(State.ParryHigh, highParry, lowParry, high));
    //             break;
    //         case State.ParryMedium:
    //             StartCoroutine(Attack(State.ParryMedium, medParry, lowParry, medium));
    //             break;
    //         case State.ParryLow:
    //             StartCoroutine(Attack(State.ParryLow, lowParry, highParry, low));
    //             break;
    //         default:
    //             enemySprite.sprite = idle;
    //             break;
    //     }
    // }

    // private IEnumerator Attack(State enemyState, Sprite startStance, Sprite endStance, Color color)
    // {
    //     btnIndicator.ShowKey(enemyState);
    //     enemySprite.color = color;
    //     enemySprite.sprite = startStance;
    //     windupSlider.gameObject.SetActive(true);
    //     for(float i = 0; i < timeInterval; i += Time.deltaTime)
    //     {
    //         windupSlider.value = i / timeInterval;
    //         yield return null;
    //     }
    //     windupSlider.gameObject.SetActive(false);
    //     //yield return new WaitForSeconds(timeInterval); // After playtest 1, make these windows smaller
    //     enemySprite.sprite = endStance;
    //     btnIndicator.HideKey();
    //     yield return new WaitForSeconds(0.2f);
    //     enemySprite.sprite = idle;
    //     enemySprite.color = originalColor;
    // }

}
