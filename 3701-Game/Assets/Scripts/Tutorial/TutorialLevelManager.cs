using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private GameObject HUD;
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
        yield return ShowTutorial("end");
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

        Time.timeScale = 1f;
        musicManager.musicPlayEvent.setPaused(false);
        gameMenu.pausable = true;


        if (stepIndex >= steps.Count)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            HUD.SetActive(false);
            tutorialEndMenu.SetActive(true);
            yield return new WaitForSecondsRealtime(1.7f);
            StartCoroutine(EndTutorial());
        }
        


    }

    private IEnumerator WaitForAction(TutorialAction action)
    {
        gameMenu.pausable = false;
        switch (action)
        {
            case TutorialAction.ParryHigh:
                yield return new WaitUntil(() => parryHigh.IsPressed());
                playerInput.inputsDisabled = true;
                break;

            case TutorialAction.ParryMedium:
                yield return new WaitUntil(() => parryMedium.IsPressed());
                playerInput.inputsDisabled = true;
                break;

            case TutorialAction.ParryLow:
                yield return new WaitUntil(() => parryLow.IsPressed());
                playerInput.inputsDisabled = true;
                break;

            case TutorialAction.EngageParry:
                yield return new WaitUntil(() => engageParry.IsPressed());
                playerInput.inputsDisabled = false;
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
                ActivateMenu(feintMenu);
                break;
            case ("dodge"):
                ActivateMenu(dodgeMenu);
                break;
            case ("perfect"):
                ActivateMenu(perfectMenu);
                break;
            case ("end"):
                ActivateMenu(tutorialEndMenu);
                break;
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) ||Input.GetKeyDown(KeyCode.KeypadEnter));

        HUD.SetActive(true);
        Time.timeScale = 1.0f;
        musicManager.musicPlayEvent.setPaused(false);
        feintMenu.SetActive(false);
        dodgeMenu.SetActive(false);
        perfectMenu.SetActive(false);
        gameMenu.pausable = true;
        playerInput.inputsDisabled = false;
    }

    private void ActivateMenu(GameObject menu)
    {
        menu.SetActive(true);
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
