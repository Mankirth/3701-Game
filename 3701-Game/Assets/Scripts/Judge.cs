using System.Data;
using System.Diagnostics;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public State goalState, playerState, beatState;
    public int goalBeat;
    public Metronome metronome;
    public MusicManager musicManager;

    public void Evaluate()
    {
        beatState = musicManager.BeatMap(); // Returns stance mapped to beat interval. Use this wherever you need to

        if (playerState == goalState && metronome.activeBeat == goalBeat)
        {
            UnityEngine.Debug.Log("Beat Match!!");
        }
        else
        {
            UnityEngine.Debug.Log("Beat Fail!!");
        }
    }

    void UpdateGoals(State state, int beat)
    {
        goalState = state;
        goalBeat = beat;
    }
}