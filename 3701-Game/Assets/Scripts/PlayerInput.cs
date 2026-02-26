using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public ParticleSystem parrySparks;
    
    public State playerState;
    InputAction parryHigh, parryMedium, parryLow;
    [SerializeField]
    private float parryLengthBeats = 2;
    [SerializeField]
    private MusicManager musicManager;
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle, highEnd, medEnd, lowEnd;
    private SfxManager sfxManager;

    [SerializeField]
    private Transform parryPos, defaultPos;
   
    void Start()
    {
        playerState = State.Idle;
        //Initialize Inputs
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        playerSprite = GetComponent<SpriteRenderer>();
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();
    
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
        yield return new WaitForSeconds(60 / musicManager.metroTempo * parryLengthBeats);

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

        if (playerState == State.Hurting)
            yield return null;


        sfxManager.QueueSound(false, sfxManager.parry);
        //Hard coding parry sparks to move vertically based on parry stance (sorry! we can fix this later!)
        if (playerState == State.ParryHigh)
        {
            playerSprite.sprite = highEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, 0.2f, parrySparks.transform.localPosition.z);
           
        }
        else if (playerState == State.ParryMedium)
        {
            playerSprite.sprite = medEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, 0f, parrySparks.transform.localPosition.z);

        }
        else if (playerState == State.ParryLow)
        {
            playerSprite.sprite = lowEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, -0.2f, parrySparks.transform.localPosition.z);

        }
        transform.position = parryPos.position;
        parrySparks.Play();
        yield return new WaitForSeconds(0.2f);
        transform.position = defaultPos.position;

        //Deactivate Parry
        ToIdle();
    }
}