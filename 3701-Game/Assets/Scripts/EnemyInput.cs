using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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

    private Animator enemyAnimator;
    [SerializeField]
    private Animator camAnimator;

    public ButtonIndicator btnIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        tempState = beatState;
        originalColor = enemySprite.color;
    }

    void Update()
    {
        beatState = musicManager.BeatMap();
        timeInterval = musicManager.timeInterval;
        if (tempState != beatState)
        {
            CheckBeatMap();
            tempState = beatState;
        }
    }

    public void CheckBeatMap()
    {
        
        switch (beatState)
        {
            case State.ParryHigh:
                StartCoroutine(Attack(State.ParryHigh, highParry, lowParry, high));
                break;
            case State.ParryMedium:
                StartCoroutine(Attack(State.ParryMedium, medParry, lowParry, medium));
                break;
            case State.ParryLow:
                StartCoroutine(Attack(State.ParryLow, lowParry, highParry, low));
                break;
            default:
                enemySprite.sprite = idle;
                break;
        }
    }

    private IEnumerator Attack(State enemyState, Sprite startStance, Sprite endStance, Color color)
    {
        btnIndicator.ShowKey(enemyState);
        enemySprite.color = color;
        enemySprite.sprite = startStance;
        
        yield return new WaitForSeconds(timeInterval); // After playtest 1, make these windows smaller
        enemySprite.sprite = endStance;
        btnIndicator.HideKey();
        yield return new WaitForSeconds(0.2f);
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;

        //enemyAnimator.SetBool("parryHigh", true);

    }

}
