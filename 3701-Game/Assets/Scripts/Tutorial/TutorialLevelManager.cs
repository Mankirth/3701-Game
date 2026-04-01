using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static TutorialStep;

public class TutorialLevelManager : MonoBehaviour
{
    public MusicManager musicManager;
    public GameObject tutorialEndMenu, feintMenu, dodgeMenu, perfectMenu;
    private InputAction parryHigh, parryMedium, parryLow, engageParry;

    [SerializeField]
    private PlayerInput playerInput;
    public bool hasDodged;
    public bool feintOccured;
    public bool perfectOccured;

    public List<TutorialStep> steps;
    public int stepIndex = 0;

    public GameMenu gameMenu;
    public void Start()
    {
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        engageParry = InputSystem.actions.FindAction("EngageParry");
    }
    private void OnEnable()
    {
        EnemyInput.OnAttackStarted += HandleAttackStarted;
        EnemyInput.OnWindupProgress += HandleWindup;
        EnemyInput.OnFeintWindow += HandleFeint;
        Judge.OnPlayerDodged += HandleDodge;
        GameManager.OnPerfectParry += HandlePerfect;
    }

    private void OnDisable()
    {
        EnemyInput.OnAttackStarted -= HandleAttackStarted;
        EnemyInput.OnWindupProgress -= HandleWindup;
        EnemyInput.OnFeintWindow -= HandleFeint;
        Judge.OnPlayerDodged -= HandleDodge;
        GameManager.OnPerfectParry -= HandlePerfect;
    }

    public IEnumerator EndTutorial()
    {
        tutorialEndMenu.SetActive(true);
        gameMenu.pausable = false;
        yield return new WaitForSeconds(3.0f);
        gameMenu.pausable = true;
        tutorialEndMenu.SetActive(false);
    }


    public IEnumerator RunTutorial()
    {
        gameMenu.pausable = false;
        Time.timeScale = 0f;
        musicManager.musicPlayEvent.setPaused(true);

        if (stepIndex < steps.Count)
        {
            TutorialStep step = steps[stepIndex];
            step.tutorialUI.SetActive(true);
            stepIndex++;
            Debug.Log("STEP INDEX: " + stepIndex);
            yield return WaitForAction(step.action);

            
            step.tutorialUI.SetActive(false);
            
        }
        if (stepIndex >= steps.Count)
        {
            StartCoroutine(EndTutorial());
        }
        

        Time.timeScale = 1f;
        musicManager.musicPlayEvent.setPaused(false);
        gameMenu.pausable = true;
    }

    private IEnumerator WaitForAction(TutorialAction action)
    {
        gameMenu.pausable = false;
        switch (action)
        {
            case TutorialAction.ParryHigh:
                yield return new WaitUntil(() => parryHigh.IsPressed());
                break;

            case TutorialAction.ParryMedium:
                yield return new WaitUntil(() => parryMedium.IsPressed());
                break;

            case TutorialAction.ParryLow:
                yield return new WaitUntil(() => parryLow.IsPressed());
                break;

            case TutorialAction.EngageParry:
                yield return new WaitUntil(() => engageParry.IsPressed());
                break;

            case TutorialAction.PressEnter:
                yield return new WaitUntil(() =>
                    Input.GetKeyDown(KeyCode.Return) ||
                    Input.GetKeyDown(KeyCode.KeypadEnter));
                break;
        }
        gameMenu.pausable = true;
    }

    public IEnumerator ShowTutorial(string type)
    {
        gameMenu.pausable = false;
        Time.timeScale = 0.0f;
        musicManager.musicPlayEvent.setPaused(true);
        switch (type){
            case ("feint"):
                feintMenu.SetActive(true);
                break;
            case ("dodge"):
                dodgeMenu.SetActive(true);
                break;
            case ("perfect"):
                perfectMenu.SetActive(true);
                break;
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
        
        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        feintMenu.SetActive(false);
        dodgeMenu.SetActive(false);
        perfectMenu.SetActive(false);
        gameMenu.pausable = true;
    }


    private void HandleFeint()
    {
        if (!feintOccured)
        {
            StartCoroutine(ShowTutorial("feint"));
            feintOccured = true;
        }
    }

    private void HandleWindup(float progress)
    {
        if (progress >= 0.8f)
        {
            if (stepIndex == 1 || stepIndex == 3 || stepIndex == 5)
            {
                StartCoroutine(RunTutorial());
            }
        }
    }

    private void HandleAttackStarted()
    {
        if (stepIndex <= 5)
        {
            StartCoroutine(RunTutorial());
        }
    }

    private void HandleDodge()
    {
        if (!hasDodged)
        {
            Debug.Log("You dodged, wowwww");
            StartCoroutine(ShowTutorial("dodge"));
            hasDodged = true;
        }

    }

    private void HandlePerfect()
    {
        if (!perfectOccured)
        {
            StartCoroutine(ShowTutorial("perfect"));
            perfectOccured = true;
        }
    }
}
