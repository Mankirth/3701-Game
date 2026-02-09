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
    private Sprite highParry, medParry, lowParry, idle;


   
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
        if (playerState != State.Idle)
            return;
        else if (parryHigh.WasPressedThisFrame()){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryHigh, highParry));
        }
        else if (parryMedium.WasPressedThisFrame()){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryMedium, medParry));
        }
        else if (parryLow.WasPressedThisFrame()){
            StopAllCoroutines();
            StartCoroutine(Parry(State.ParryLow, lowParry));
        }
    }

    private IEnumerator Parry(State height, Sprite stance)
    {
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
}