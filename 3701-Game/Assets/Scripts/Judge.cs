using System.Data;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State playerState, beatState;
    public PlayerInput player;
    private bool mustEngage;

    public GameManager gameManager;

    public EnemyInput enemy;
    public Health health;

    public SfxManager sfxManager;

    public PlayerSettings settings;



    public void Evaluate(State beatState, bool playSound)
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
        StopAllCoroutines();
        StartCoroutine(player.SuccessParry());
    }

    private void Dodge()
    {
        sfxManager.QueueSound(false, sfxManager.playerDodge);
        gameManager.DeductScore(450, "dodge");
        StopAllCoroutines();
        StartCoroutine(health.Hit());
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