using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayLevel : MonoBehaviour
{
    [SerializeField]
    private NarrativeProgression progressionTracker;

    [SerializeField]
    private GameObject replayBtn;

    [SerializeField]
    private Rivals rival;
    private enum Rivals
    {
        Swan,
        Prince,
        Drunkard
    }

    void Update()
    {
        bool canReplay = rival switch
        {
            Rivals.Swan =>
                progressionTracker.swanDialogueState == NarrativeProgression.NPCDialogueState.PostFight,

            Rivals.Prince =>
                progressionTracker.princeDialogueState == NarrativeProgression.NPCDialogueState.PostFight,

            _ => false
        };

        replayBtn.SetActive(canReplay);

    }


    public void PlayRival(string name)
    {
        SceneManager.LoadScene(name);
    }
}
