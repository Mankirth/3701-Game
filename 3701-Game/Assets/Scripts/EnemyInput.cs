using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EnemyInput : MonoBehaviour
{
    public State beatState;
    private float timeInterval;
    [SerializeField]
    private MusicManager musicManager;

    public Sprite highParry, medParry, lowParry, idle, strike;
    [SerializeField]
    private GameObject highAttack, medAttack, lowAttack;

    [SerializeField]
    private Animator enemyDeath;
    private SpriteRenderer enemySprite;
    private State tempState;
    private Color originalColor;
    public Color high, medium, low;

    public ButtonIndicator btnIndicator;
    [SerializeField]
    private Slider windupSlider;

    public float windupValue = 0;

    public Transform attackPos, defendPos;
    public GameObject outline, loseRed;
    private SfxManager sfxManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        //outline.SetActive(false);
        tempState = beatState;
        originalColor = enemySprite.color;
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        
    }

    public void StartAttack(State state, float beats)
    {
        if(state != State.Idle && state != State.Hurting)
            sfxManager.QueueSound(true, sfxManager.windUp, (int)state);
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
        enemySprite.sprite = startStance;

        //activate outline
        // outline.SetActive(true);
        // outline.GetComponent<SpriteRenderer>().sprite = enemySprite.sprite;
        // outline.GetComponent<SpriteRenderer>().color = color;
        // outline.transform.position = Camera.main.transform.position + (Camera.main.transform.position - transform.position);
        
        for(float i = 0; i < outBeat; i += Time.deltaTime)
        {
            windupValue = windupSlider.value;
            windupSlider.value = i / outBeat;
            // outline.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Math.Max(0,(outBeat - (i*1.25f)) / outBeat));
            // if((outline.transform.position - transform.position).magnitude > 0.1f)
            //     outline.transform.position = Camera.main.transform.position + ((Camera.main.transform.position - transform.position) * 0.8f) + (i / outBeat * 2 * (transform.position - Camera.main.transform.position));
            yield return null;
        }

        //outline.SetActive(false);
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
        enemyDeath.enabled = true;
        loseRed?.SetActive(true);
    }
}
