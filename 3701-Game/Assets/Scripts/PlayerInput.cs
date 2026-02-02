using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public State playerState;
    InputAction parryHigh, parryMedium, parryLow;
    [SerializeField]
    private float parryTimeMs = 200f;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private Color highCol, medCol, lowCol;
    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle;


   
    void Start()
    {
        playerState = State.Idle;
        //Initialize Inputs
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
    }

    // Update is called once per frame
    void Update()
    {
        //Check Input
        if (playerState != State.Idle)
            return;
        else if (parryHigh.WasPressedThisFrame())
            StartCoroutine(Parry(State.ParryHigh, highParry));
        else if (parryMedium.WasPressedThisFrame())
            StartCoroutine(Parry(State.ParryMedium, medParry));
        else if (parryLow.WasPressedThisFrame())
            StartCoroutine(Parry(State.ParryLow, lowParry));
    }

    private IEnumerator Parry(State height, Sprite stance)
    {
        //Activate Parry
        playerState = height;
        sprite.sprite = stance;
        Debug.Log("Entering State: " + height.ToString());

        //Wait
        yield return new WaitForSeconds(parryTimeMs / 1000);

        //Deactivate Parry
        playerState = State.Idle;
        sprite.sprite = idle;
        Debug.Log("Entering State: Idle");
    }
}