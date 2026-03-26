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

    public ButtonIndicator btnIndicator;
    [SerializeField]
    private Slider windupSlider;

    public float windupValue = 0;

    public Transform attackPos, defendPos;
    public GameObject loseRed;

    public bool striking;

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
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        
    }

    public void StartAttack(State state, float beats, bool isFeint)
    {
        if (state != State.Idle && state != State.Hurting)
        {
            sfxManager.QueueSound(true, sfxManager.windUp, (int)state);
            
        }
        beatState = musicManager.beatStance;
        timeInterval = musicManager.timeInterval;
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(CancelAttacks(Attack(State.ParryHigh, highParry, strike, highAttack, 60 / musicManager.metroTempo * beats, isFeint)));
                break;
            case State.ParryMedium:
                StartCoroutine(CancelAttacks(Attack(State.ParryMedium, medParry, strike, medAttack, 60 / musicManager.metroTempo * beats, isFeint)));
                break;
            case State.ParryLow:
                StartCoroutine(CancelAttacks(Attack(State.ParryLow, lowParry, strike, lowAttack, 60 / musicManager.metroTempo * beats, isFeint)));
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

    private IEnumerator Attack(State state, Sprite startStance, Sprite endStance, GameObject followThrough, float outBeat, bool isFeint)
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

            if (isFeint && !feintEventTriggered && progress >= 0.6f)
            {
                feintEventTriggered = true;
                OnFeintWindow?.Invoke();
            }
            if (!engageWindowTriggered && progress >= 0.8f)
            {
                engageWindowTriggered = true;

                OnEngageWindow?.Invoke();

                if (settings.parryEngage == PlayerSettings.ParryEngage.Enabled)
                {
                    btnIndicator.ShowEngageKey();
                    btnIndicator.HideKey();
                }
            }

            yield return null;
        }

        // Attack release (if not feint)
        if (!isFeint)
        {
            striking = true;

            windupSlider.gameObject.SetActive(false);
            enemySprite.sprite = endStance;
            btnIndicator.HideEngageKey();
            transform.position = attackPos.position;

            OnAttackReleased?.Invoke();

            GameObject.Find("Judge").GetComponent<Judge>().Evaluate(state, false);

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
        if(narProg != null)
            narProg.currRivalToFight = NarrativeProgression.FightableRival.Prince;
        loseRed?.SetActive(true);
    }
}
