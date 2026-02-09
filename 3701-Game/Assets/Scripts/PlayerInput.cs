using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public State playerState;
    InputAction parryHigh, parryMedium, parryLow;
    [SerializeField]
    private float parryTimeMs = 200f;
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle, highEnd, medEnd, lowEnd;


   
    void Start()
    {
        playerState = State.Idle;
        //Initialize Inputs
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
            return;
        //Check Input
        // if (playerState != State.Idle)
        //     return;
        if (parryHigh.WasPressedThisFrame() && playerState != State.ParryHigh){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryHigh, highParry));
        }
        else if (parryMedium.WasPressedThisFrame() && playerState != State.ParryMedium){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryMedium, medParry));
        }
        else if (parryLow.WasPressedThisFrame() && playerState != State.ParryLow){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryLow, lowParry));
        }
    }

    private IEnumerator Parry(State height, Sprite stance)
    {
        if(playerState == State.Dead)
            yield return null;
        //Activate Parry
        playerState = height;
        playerSprite.sprite = stance;

        //Wait
        yield return new WaitForSeconds(parryTimeMs / 1000);

        //Deactivate Parry
        ToIdle();
    }

    public void ToIdle()
    {
        playerState = State.Idle;
        playerSprite.sprite = idle;
    }

    public IEnumerator SuccessParry()
    {
        if (playerState == State.Dead)
            yield return null;

        if (playerState == State.ParryHigh)
        {
            playerSprite.sprite = highEnd;
        }
        else if (playerState == State.ParryMedium)
        {
            playerSprite.sprite = medEnd;
        }
        else if (playerState == State.ParryLow)
        {
            playerSprite.sprite = lowEnd;
        }
        yield return new WaitForSeconds(0.2f);

        //Deactivate Parry
        ToIdle();
    }
}