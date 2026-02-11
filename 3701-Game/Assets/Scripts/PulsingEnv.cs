using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class PulsingEnv : MonoBehaviour
{
    [SerializeField]
    private Metronome metronome;
    [SerializeField]
    private int beatInterval = 2; // Maybe switch to enum, not needed if sticking to 4/4

    [SerializeField]
    private float targetScale = 0.9f;
    private float originalScale = 1.0f;

    private int lastBeat;

    private bool maxReached = false;

     void Start()
    {
        originalScale = transform.localScale.x;
    }
    void Update()
    {
        if (transform.localScale.x < targetScale && !maxReached && metronome.activeBeat != lastBeat && metronome.activeBeat % beatInterval == 0)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            lastBeat = metronome.activeBeat;
        }
        else
        {
            maxReached = true;
        }

        if (transform.localScale.x > originalScale && maxReached && metronome.activeBeat != lastBeat && metronome.activeBeat % beatInterval == 0)
        {
            transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            lastBeat = metronome.activeBeat;
        }
        else
        {
            maxReached = false;
        }
    }

}
