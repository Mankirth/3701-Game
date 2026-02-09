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
    float targetScale = 0.9f;

    private int lastBeat;

    private bool maxReached = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < targetScale && !maxReached && metronome.activeBeat != lastBeat && metronome.activeBeat % 2 == 0)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            lastBeat = metronome.activeBeat;
        }
        else
        {
            maxReached = true;
        }

        if (transform.localScale.x > 0.5f && maxReached && metronome.activeBeat != lastBeat && metronome.activeBeat % 2 == 0)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            lastBeat = metronome.activeBeat;
        }
        else
        {
            maxReached = false;
        }

        //StartCoroutine(Pulse(objects[1]);
    }

    //public IEnumerator Pulse(Transform backgroundObject)
    //{
        

    //    if (backgroundObject.localScale.x < targetScale)
    //    {
    //        backgroundObject.localScale += new Vector3(targetScale, targetScale, targetScale);
    //    }
    //    yield return new WaitForSeconds(0.1f);

    //    if (backgroundObject.localScale.x > 0.5f)
    //    {
    //        backgroundObject.localScale += new Vector3(0.5f, 0.5f, 0.5f);
    //    }

    //}
}
