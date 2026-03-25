using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialLevelManager : MonoBehaviour
{
    public GameObject[] tutorialUI; // Each UI should have a little prompt that says "Oh press W to parry high, watch their stance!"
    public MusicManager musicManager;
    public GameObject tutorialEndMenu, tutorialFeintMenu;
    private InputAction parryHigh, parryMedium, parryLow, engageParry;
    [SerializeField]
    private PlayerInput playerInput;
    private bool hasDodged;
    public bool feintOccured;
    public int index = 0;

    public void Start()
    {
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        engageParry = InputSystem.actions.FindAction("EngageParry");
    }

    //public void Update()
    //{
    //    if (musicManager.timelineInfo.currentBar == 9 && musicManager.timelineInfo.currentBeat == 4)
    //    {
    //        playerInput.inputEnabled = false;
    //        Debug.Log(":GOOF");
    //    }
    //}

    public IEnumerator EndTutorial()
    {
        tutorialEndMenu.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        tutorialEndMenu.SetActive(false);
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
            StartCoroutine(EndTutorial());
        }
        if (index == 6)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
        }
        
        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        tutorialUI[index].SetActive(false);
        index++;
        Debug.Log("INDEX: " + index);
    }

    public IEnumerator FeintTutorial()
    {
        Time.timeScale = 0.0f;
        musicManager.musicPlayEvent.setPaused(true);
        tutorialFeintMenu.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
        feintOccured = true;
        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        tutorialFeintMenu.SetActive(false);
    }
}
