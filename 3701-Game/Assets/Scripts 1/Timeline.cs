using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// Timeline which references music manager array to obtain beat stances. Timeline should progress to the beat. Think LINKED LIST
// Current beat event, next beat event, and reference to the beat event after that
public class Timeline : MonoBehaviour
{
    [SerializeField]
    private MusicManager musicManager;

    public Sprite lowParry, medParry, highParry;

    [SerializeField] private Image[] inputImages;

    public List<State> beatStances = new List<State>();

    private void Start()
    {
        //Debug.Log(beatStances.Count);

        for (int i = 0; i < musicManager.beatEvents.Count; i++)
        {
            beatStances[i] = (musicManager.beatEvents[i].stance);
        }

    }
    // To swap to the next input, check if the enemy is in that stance? If the enemy is then go to the next input?
    // Could also retrieve currentbeat from music manager, but would need to account for different beat event lengths
    // Use beatstate list? Has the beat intervals and make the timeline switch to next beat after interval length. Would need to keep track of beat here as well

    private void Update()
    {
        if (beatStances.Count <= 0)
            return;

        if (musicManager.beatStance == beatStances[0])
        {
            beatStances.RemoveAt(0);
        }

        // NOT FINAL RENDITION AT ALL, THIS IS VERY INEFFICIENT, FIX ASAP
        for (int i = 0; i < inputImages.Length && i < beatStances.Count; i++)
        {
            if (ShowSprite(beatStances[i]) != null)
            {
                inputImages[i].sprite = ShowSprite(beatStances[i]);
            }
        }
    }

    private Sprite ShowSprite(State state)
    {
        switch (state)
        {
            case State.ParryLow:
                return lowParry;
            case State.ParryMedium:
                return medParry;
            case State.ParryHigh:
                return highParry;
            default:
                return null;
        }
    }

}
