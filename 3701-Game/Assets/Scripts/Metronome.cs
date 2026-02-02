using UnityEngine;
using UnityEngine.Events;

public class Metronome : MonoBehaviour
{
    public int bpm = 60, activeBeat = 0;
    private float beatDurationMs, nextBeatPosition, songPosition = 0, activeBeatStartPosition = 0, activeBeatEndPosition = 0;
    private int lastBeat = 0;
    public Judge judge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatDurationMs = 60 / bpm * 1000;
        nextBeatPosition = beatDurationMs;
        activeBeatStartPosition = nextBeatPosition;
        activeBeatEndPosition = nextBeatPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        songPosition += Time.deltaTime * 1000;
        if (songPosition >= nextBeatPosition)
        {
            activeBeat = ((activeBeat + 1) % 4) + 1;
            nextBeatPosition += beatDurationMs;
            judge.Evaluate();
        }
    }
}