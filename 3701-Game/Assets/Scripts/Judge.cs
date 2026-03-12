using System.Data;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State playerState, beatState;
    public PlayerInput player;
    public GameManager gameManager;
    public EnemyInput enemy;
    public Health health;
    public SfxManager sfxManager;

    public PlayerSettings settings;



    public void Evaluate(State beatState, bool playSound)
    {
        //beatState = enemy.beatState; // Returns stance mapped to beat interval. Use this wherever you need to
        playerState = player.playerState;
        //settings.difficulty == PlayerSettings.Difficulty.Hard; // USE THIS TO CHECK DIFFICULTY, IF ON HARD MODE (WHICH WILL SWITCH TO NORMAL) PLAYER MUST ENGAGE
        //Debug.Log("Player state: " + playerState + " Beat state: " + beatState + "Engaging: " + player.isEngaging);
        if (((playerState == beatState || beatState == State.Idle) && player.isEngaging))
        {
            Debug.Log("Beat Match!!");

            sfxManager.QueueSound(false, sfxManager.parry);
            gameManager.AddParryScore();
            StopAllCoroutines();
            StartCoroutine(player.SuccessParry());


        }
        else {
            
            sfxManager.QueueSound(false, sfxManager.playerDodge);
            gameManager.DeductScore(450, "dodge");
            StopAllCoroutines();
            StartCoroutine(health.Hit());

        }
    }

    public void CheckTiming()
    {
        if (!enemy.striking)
        {
            Debug.Log("Early");
            gameManager.DeductScore(100, "miss");
        }
    }
}