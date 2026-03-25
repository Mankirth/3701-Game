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
    public GameObject loseRed;

    public bool striking;

    [SerializeField]
    private PlayerSettings settings;
    
    private SfxManager sfxManager;
    [SerializeField]
    private bool isTutorial;
    [SerializeField]
    private TutorialLevelManager tutorialManager;
    [SerializeField]
    private NarrativeProgression narProg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        //outline.SetActive(false);
        tempState = beatState;
        originalColor = enemySprite.color;
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
        if (isTutorial)
        {
            tutorialManager = GameObject.FindAnyObjectByType<TutorialLevelManager>();
        }
        
    }

    public void StartAttack(State state, float beats, bool isFeint)
    {
        if (state != State.Idle && state != State.Hurting)
        {
            sfxManager.QueueSound(true, isFeint ? sfxManager.feint:sfxManager.windUp, (int)state);
        }
        beatState = musicManager.beatStance;
        timeInterval = musicManager.timeInterval;
        switch (state)
        {
            case State.ParryHigh:
                StartCoroutine(CancelAttacks(Attack(State.ParryHigh, highParry, strike, highAttack, high, 60 / musicManager.metroTempo * beats, isFeint)));
                break;
            case State.ParryMedium:
                StartCoroutine(CancelAttacks(Attack(State.ParryMedium, medParry, strike, medAttack, medium, 60 / musicManager.metroTempo * beats, isFeint)));
                break;
            case State.ParryLow:
                StartCoroutine(CancelAttacks(Attack(State.ParryLow, lowParry, strike, lowAttack, low, 60 / musicManager.metroTempo * beats, isFeint)));
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
        enemySprite.color = originalColor;
        transform.position = defendPos.position;
        yield return StartCoroutine(enumerator);
    }

    private IEnumerator Attack(State state, Sprite startStance, Sprite endStance, GameObject followThrough, Color color, float outBeat, bool isFeint)
    {
        btnIndicator.ShowKey(state);
        btnIndicator.HideEngageKey();
        enemySprite.sprite = startStance;

        if (isTutorial && tutorialManager.index == 0)
        {
            StartCoroutine(tutorialManager.ResumeTutorial());
        }

        for (float i = 0; i < outBeat; i += Time.deltaTime)
        {
            windupValue = windupSlider.value;
            windupSlider.value = i / outBeat;


            if (i / outBeat >= 0.8 && isTutorial && tutorialManager.index == 1) // Find away to avoid doing this conditional in non-tutorial levels
            {
                StartCoroutine(tutorialManager.ResumeTutorial());
            }
            if (i / outBeat >= 0.8 && settings.parryEngage == PlayerSettings.ParryEngage.Enabled) // Later, change 0.8 to a variable that can be modified in inspector
            {
                btnIndicator.ShowEngageKey();
                btnIndicator.HideKey();
            }
            yield return null;
        }
        if(!isFeint){
            striking = true;

            windupSlider.gameObject.SetActive(false);
            enemySprite.sprite = endStance;
            btnIndicator.HideEngageKey();
            transform.position = attackPos.position;

            GameObject.Find("Judge").GetComponent<Judge>().Evaluate(state, false);

            followThrough.SetActive(true);

            yield return new WaitForSeconds(60 / musicManager.metroTempo);
            striking = false;
            transform.position = defendPos.position;
        }
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;
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
