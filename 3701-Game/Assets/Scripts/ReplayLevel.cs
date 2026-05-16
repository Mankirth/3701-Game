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
        if (rival == Rivals.Swan && progressionTracker.swanDialogueState == NarrativeProgression.NPCDialogueState.PostFight)
        {
            replayBtn.SetActive(true);
        }
        else
        {
            replayBtn.SetActive(false);
        }
    }


    public void PlayRival(string name)
    {
        SceneManager.LoadScene(name);
    }
}
