using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialLevelManager : MonoBehaviour
{
    public GameObject[] tutorialUI;
    public MusicManager musicManager;
    private InputAction parryHigh, parryMedium, parryLow, engageParry;
    public int index = 0;

    public void Start()
    {
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        engageParry = InputSystem.actions.FindAction("EngageParry");
    }

    public IEnumerator ResumeTutorial()
    {
        Time.timeScale = 0.0f;
        musicManager.musicPlayEvent.setPaused(true);
        tutorialUI[index].SetActive(true);
        if (index == 0) {
            yield return new WaitUntil(() => parryLow.IsPressed());
        }
        if (index == 1)
        {
            yield return new WaitUntil(() => engageParry.IsPressed());
        }

        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        tutorialUI[index].SetActive(false);
        index++;
    }
}
