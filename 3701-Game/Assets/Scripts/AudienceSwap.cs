using UnityEngine;

public class AudienceSwap : MonoBehaviour
{
    [SerializeField]
    private Metronome metronome;
    [SerializeField]
    private GameObject crowd1, crowd2;
    [SerializeField]
    private int beatInterval = 2; // Maybe switch to enum, not needed if sticking to 4/4

    private float lastBeat;

    private bool swapped = false;

    void Update()
    {
        if (metronome.activeBeat != lastBeat && metronome.activeBeat % beatInterval == 0)
        {
            crowd1.SetActive(swapped);
            crowd2.SetActive(!swapped);
            swapped = !swapped;
            lastBeat = metronome.activeBeat;
        }
    }
}
