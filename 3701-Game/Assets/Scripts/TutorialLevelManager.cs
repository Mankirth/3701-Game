using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialLevelManager : MonoBehaviour
{
    public GameObject[] tutorialUI; // Each UI should have a little prompt that says "Oh press W to parry high, watch their stance!"
    public MusicManager musicManager;
    public GameObject tutorialEndMenu;
    private InputAction parryHigh, parryMedium, parryLow, engageParry;
    public int index = 0;

    public void Start()
    {
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        engageParry = InputSystem.actions.FindAction("EngageParry");
    }

    public void EndTutorial()
    {
        tutorialEndMenu.SetActive(true);
    }

    public void DisableInputs()
    {

    }
    public IEnumerator ResumeTutorial()
    {
        Time.timeScale = 0.0f;
        musicManager.musicPlayEvent.setPaused(true);
        tutorialUI[index].SetActive(true);
        if (index == 0) {
            yield return new WaitUntil(() => parryHigh.IsPressed());
        }
        if (index == 1)
        {
            yield return new WaitUntil(() => engageParry.IsPressed());
        }

        if (index == 2)
        {
            yield return new WaitUntil(() => parryMedium.IsPressed());
        }

        if (index == 3)
        {
            yield return new WaitUntil(() => engageParry.IsPressed());
        }

        if (index == 4)
        {
            yield return new WaitUntil(() => parryLow.IsPressed());
        }

        if (index == 5)
        {
            yield return new WaitUntil(() => engageParry.IsPressed());
            EndTutorial();
        }
        
        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        tutorialUI[index].SetActive(false);
        index++;
        Debug.Log("INDEX: " + index);
    }
}
