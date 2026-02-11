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
    private Sprite highParry, medParry, lowParry, idle;

    private SpriteRenderer enemySprite;
    private State tempState;
    private Color originalColor;
    [SerializeField]
    private Color high, medium, low;

    public ButtonIndicator btnIndicator;
    [SerializeField]
    private Slider windupSlider;
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
        CancelAttacks();
        beatState = musicManager.beatStance;
        timeInterval = musicManager.timeInterval;
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(Attack(State.ParryHigh, highParry, lowParry, high, 60 / musicManager.metroTempo * beats));
                break;
            case State.ParryMedium:
                StartCoroutine(Attack(State.ParryMedium, medParry, lowParry, medium, 60 / musicManager.metroTempo * beats));
                break;
            case State.ParryLow:
                StartCoroutine(Attack(State.ParryLow, lowParry, highParry, low, 60 / musicManager.metroTempo * beats));
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

    private IEnumerator Attack(State state, Sprite startStance, Sprite endStance, Color color, float outBeat)
    {
        btnIndicator.ShowKey(state);
        enemySprite.color = color;
        enemySprite.sprite = startStance;
        windupSlider.gameObject.SetActive(true);

        for(float i = 0; i < outBeat; i += Time.deltaTime)
        {
            windupSlider.value = i / outBeat;
            yield return null;
        }

        GameObject.Find("Judge").GetComponent<Judge>().Evaluate(state);
        windupSlider.gameObject.SetActive(false);
        enemySprite.sprite = endStance;
        btnIndicator.HideKey();

        yield return new WaitForSeconds(0.2f);
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;
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
