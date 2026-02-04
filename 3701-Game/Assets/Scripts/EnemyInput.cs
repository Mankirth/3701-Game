using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyInput : MonoBehaviour
{
    public State beatState;
    [SerializeField]
    private MusicManager musicManager;

    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle;

    private SpriteRenderer enemySprite;
    private State tempState;
    private Color originalColor;
    [SerializeField]
    private Color high, medium, low;
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
        Debug.Log("TMEMEEOPWJA IPHDWP EHWA : " + tempState);
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
                StartCoroutine(Attack(State.ParryHigh, medParry, lowParry, medium));
                break;
            case State.ParryLow:
                StartCoroutine(Attack(State.ParryHigh, lowParry, highParry, low));
                break;
            default:
                enemySprite.sprite = idle;
                break;
        }
    }

    private IEnumerator Attack(State enemyState, Sprite startStance, Sprite endStance, Color color)
    {
        enemySprite.color = color;
        enemySprite.sprite = startStance;
        yield return new WaitForSeconds(2f);
        enemySprite.sprite = endStance;
        yield return new WaitForSeconds(0.5f);
        enemySprite.sprite = idle;
        enemySprite.color = originalColor;
        Debug.Log("WDUIADIWADHOAHDOHA AW" + beatState.ToString());

    }

}
