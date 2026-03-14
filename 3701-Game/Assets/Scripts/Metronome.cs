using System;
using UnityEngine;
using UnityEngine.Events;

public class Metronome : MonoBehaviour
{
    public float bpm = 60;
    public float activeBeat = 0;
    private float beatDurationMs, nextBeatPosition, songPosition = 0, activeBeatStartPosition = 0, activeBeatEndPosition = 0;
    private int lastBeat = 0;
    public Judge judge;

    public MusicManager musicManager;
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
        activeBeat = RoundToWholeBeat(musicManager.metroBeat);
        bpm = musicManager.metroTempo;
    }

    float RoundToWholeBeat(float beat, float delay = 0.2f) // Round beat to nearest whole number when within 0.2 of it
    {
        float nearest = Mathf.Round(beat);
        return Mathf.Abs(beat - nearest) <= delay ? nearest : beat;
    }

    public bool BeatIsWhole()
    {
        if (Mathf.Abs(activeBeat - Mathf.Round(activeBeat)) <= 0.35f)
        {
            return true;
        }
        return false;
      
    }
}