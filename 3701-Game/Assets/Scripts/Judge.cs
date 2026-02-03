using System.Data;
using System.Diagnostics;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State playerState, beatState;
    public PlayerInput player;
    public EnemyInput enemy;
    public Health health;

    public void Evaluate()
    {
        beatState = enemy.beatState; // Returns stance mapped to beat interval. Use this wherever you need to
        playerState = player.playerState;
        if (playerState == beatState || beatState == State.Idle)
        {
            UnityEngine.Debug.Log("Beat Match!!");
            player.ToIdle();
        }
        else
            StartCoroutine(health.Hit());
    }
}