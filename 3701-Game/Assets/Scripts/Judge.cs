using System;
using System.Data;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State playerState, beatState;
    private PlayerInput player;
    private bool mustEngage;

    private GameManager gameManager;

    private EnemyInput enemy;
    private Health health;

    private SfxManager sfxManager;

    public PlayerSettings settings;

    public bool isTutorial;
    public TutorialLevelManager tutorialManager;

    public static event Action OnPlayerDodged;

    void Start()
    {
        health = GameObject.Find("Player").GetComponent<Health>();
        player = GameObject.Find("Player").GetComponent<PlayerInput>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyInput>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
    }
    
    public void Evaluate(State beatState)
    {
        playerState = player.playerState;

        mustEngage = settings.parryEngage == PlayerSettings.ParryEngage.Enabled;

        if ((playerState == beatState || beatState == State.Idle) && !mustEngage)
        {
            Parry();
        }
        else if ((playerState == beatState || beatState == State.Idle) && mustEngage && player.isEngaging)
        {
            Parry();
        }
        else
        {

            Dodge();

        }
    }

    private void Parry()
    {
        sfxManager.QueueSound(false, sfxManager.parry);
        gameManager.AddParryScore();
        player.StopAllCoroutines();
        StartCoroutine(player.SuccessParry());
    }

    private void Dodge()
    {

        sfxManager.QueueSound(false, sfxManager.playerDodge);
        gameManager.DeductScore(450, "dodge");
        player.StopAllCoroutines();
        StartCoroutine(health.Hit());
        OnPlayerDodged?.Invoke();
    }
    public bool CheckTiming()
    {

        if (!enemy.striking)
        {
            Debug.Log("Early");
            gameManager.DeductScore(100, "miss");
            return false;
        }
        return true;
    }
}