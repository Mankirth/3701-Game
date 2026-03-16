using System.Data;
using System.Diagnostics;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State playerState, beatState;
    public PlayerInput player;
    public GameManager gameManager;
    public EnemyInput enemy;
    public Health health;

    public void Evaluate(State beatState)
    {
        //beatState = enemy.beatState; // Returns stance mapped to beat interval. Use this wherever you need to
        playerState = player.playerState;
        if (playerState == beatState || beatState == State.Idle)
        {
            gameManager.AddParryScore();
            UnityEngine.Debug.Log("Beat Match!!");
            StopAllCoroutines();
            StartCoroutine(player.SuccessParry());
            
        }
        else {
            gameManager.DeductFailScore();
            StopAllCoroutines();
            StartCoroutine(health.Hit());
            
        }
    }
}