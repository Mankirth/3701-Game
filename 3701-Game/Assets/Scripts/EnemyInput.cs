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

    public Sprite highParry, medParry, lowParry, idle, strike, bufferStance;
    [SerializeField]
    private GameObject highAttack, medAttack, lowAttack;

    [SerializeField]
    private Animator enemyDeath;
    private SpriteRenderer enemySprite;

    private ButtonIndicator btnIndicator;
    [SerializeField]
    private Slider windupSlider;

    public float windupValue = 0;

    public Transform attackPos, defendPos;
    public GameObject loseRed;

    public bool striking;
    public OutlineHandler outlineHandler;

    [SerializeField]
    private PlayerSettings settings;
    
    private SfxManager sfxManager;
    [SerializeField]
    private NarrativeProgression narProg;

    public static event Action OnAttackStarted;
    public static event Action<float> OnWindupProgress;
    public static event Action OnFeintWindow;
    public static event Action OnEngageWindow;
    public static event Action OnAttackReleased;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        btnIndicator = GameObject.Find("ButtonIcon").GetComponent<ButtonIndicator>();
        //outline.SetActive(false);
        // tempState = beatState;
        // originalColor = enemySprite.color;
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        
    }

    public void StartAttack(State state, float beats, bool isFient)
    {
        if (state != State.Idle && state != State.Hurting)
        {
            sfxManager.QueueSound(true, isFient ? sfxManager.feint:sfxManager.windUp, (int)state);
            enemyDeath.enabled = false;
        }
        beatState = musicManager.beatStance;
        timeInterval = musicManager.timeInterval;
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(CancelAttacks(Attack(State.ParryHigh, highParry, strike, highAttack, 60 / musicManager.metroTempo * beats, isFient)));
                break;
            case State.ParryMedium:
                StartCoroutine(CancelAttacks(Attack(State.ParryMedium, medParry, strike, medAttack, 60 / musicManager.metroTempo * beats, isFient)));
                break;
            case State.ParryLow:
                StartCoroutine(CancelAttacks(Attack(State.ParryLow, lowParry, strike, lowAttack, 60 / musicManager.metroTempo * beats, isFient)));
                break;
            case State.Hurting:
                EnemyDie();
                break;
            default:
                break;
        }
    }

    IEnumerator CancelAttacks(IEnumerator enumerator)
    {
        StopAllCoroutines();
        windupSlider.value = 0;
        windupSlider.gameObject.SetActive(false);
        btnIndicator.HideKey();
        enemySprite.sprite = idle;
        transform.position = defendPos.position;
        yield return StartCoroutine(enumerator);
    }

    private IEnumerator Attack(State state, Sprite startStance, Sprite endStance, GameObject followThrough, float outBeat, bool isFient)
    {
        btnIndicator.ShowKey(state);
        btnIndicator.HideEngageKey();
        enemySprite.sprite = startStance;

        OnAttackStarted?.Invoke(); // Start attack event, listen for individual windup, feint, and release events

        float progress = 0f;
        bool feintEventTriggered = false;
        bool engageWindowTriggered = false;


        // Windup loop
        for (float i = 0; i < outBeat; i += Time.deltaTime)
        {
            progress = i / outBeat;

            windupValue = windupSlider.value;
            windupSlider.value = progress;

            OnWindupProgress?.Invoke(progress);

            if (isFient && !feintEventTriggered && progress >= 0.6f)
            {
                feintEventTriggered = true;
                OnFeintWindow?.Invoke();
            }
            if (!engageWindowTriggered && progress >= 0.8f)
            {
                engageWindowTriggered = true;

                OnEngageWindow?.Invoke();
                if (!isFient)
                {
                    btnIndicator.ShowEngageKey();
                    
                }
                btnIndicator.HideKey();
            }
            if (progress >= 0.9f && !isFient)
            {
                enemySprite.sprite = bufferStance;
                outlineHandler.ChangeOutline(bufferStance);
            }


            yield return null;
        }
        if(!isFient){
            striking = true;
            
            windupSlider.gameObject.SetActive(false);
            enemySprite.sprite = endStance;
            btnIndicator.HideEngageKey();
            transform.position = attackPos.position;

            OnAttackReleased?.Invoke();

            GameObject.Find("Judge").GetComponent<Judge>().Evaluate(state);

            followThrough.SetActive(true);

            yield return new WaitForSeconds(60f / musicManager.metroTempo);

            striking = false;
            transform.position = defendPos.position;
        }
        enemySprite.sprite = idle;
        followThrough.SetActive(false);
    }

  
    private void EnemyDie()
    {
        enemyDeath.enabled = true;
        enemyDeath.SetTrigger("Die");
        if(narProg != null)
            narProg.currRivalToFight = NarrativeProgression.FightableRival.Prince;
        loseRed?.SetActive(true);
    }
}
