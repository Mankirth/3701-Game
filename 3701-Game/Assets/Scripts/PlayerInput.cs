using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public ParticleSystem parrySparks, musicCircle;
    
    public State playerState;
    InputAction parryHigh, parryMedium, parryLow, engageParry;
    [SerializeField]
    private float parryLengthBeats = 0.5f;
    [SerializeField]
    private MusicManager musicManager;
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Sprite highParry, medParry, lowParry, idle, highEnd, medEnd, lowEnd;
    private SfxManager sfxManager;

    [SerializeField]
    private Transform parryPos, defaultPos;
    public Animator playerAnim;

    private bool gameOver = false;

    public EnemyInput enemy;

    [HideInInspector]
    public float inputTiming;
    public bool isEngaging;

    private bool onBeat;
    private bool success;

    public Judge judge;

    public PlayerSettings playerSettings;

    public bool isTutorial;

    [HideInInspector]
    public bool inputEnabled = true;
    void Start()
    {
        playerState = State.Idle;
        //Initialize Inputs
        parryHigh = InputSystem.actions.FindAction("ParryHigh");
        parryMedium = InputSystem.actions.FindAction("ParryMedium");
        parryLow = InputSystem.actions.FindAction("ParryLow");
        engageParry = InputSystem.actions.FindAction("EngageParry");
        playerSprite = GetComponent<SpriteRenderer>();
        sfxManager = GameObject.Find("SfxManager").GetComponent<SfxManager>();

        if (isTutorial)
        {
            playerSettings.SetDifficultyPreset(PlayerSettings.Difficulty.Normal);
            playerSettings.inputIcon = PlayerSettings.InputIcon.Show;
            playerSettings.outline = PlayerSettings.Outline.Default;
        }
        else
        {
            playerSettings.inputIcon = PlayerSettings.InputIcon.Hide;
            playerSettings.outline = PlayerSettings.Outline.Fading;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("EmptyState"))
            playerAnim.enabled = false;

        //if (!inputEnabled)
        //{
        //    return;
        //}

        if (Time.timeScale == 0 && !isTutorial)
            return;
        if (playerState == State.Hurting)
            return;
        //Check Input
        
        if (!gameOver)
        {
            if (parryHigh.WasPressedThisFrame())
            {
                // Change after, updated state before coroutine to avoid mismatch during evaluation in judge
                
                StopAllCoroutines();
                StartCoroutine(Parry(State.ParryHigh, highParry));
            }
            if (parryMedium.WasPressedThisFrame())
            {
                //playerState = State.ParryMedium;
                StopAllCoroutines();
                StartCoroutine(Parry(State.ParryMedium, medParry));
            }
            if (parryLow.WasPressedThisFrame())
            {
                //playerState = State.ParryLow;
                StopAllCoroutines();
                StartCoroutine(Parry(State.ParryLow, lowParry));
            }
        }


        if (musicManager.SongEnd() == true && !gameOver)
        {
            Debug.Log("It's over");
            StopAllCoroutines();
            gameOver = true;
            Strike();
        }

  
    }

    public void Strike()
    {
        playerAnim.enabled = true;
        playerAnim.Play("StrikeAnimation", 0, 0f);
        //transform.position = strikePos.position;
    }

    private IEnumerator Parry(State height, Sprite stance)
    {
        //Activate Parry
        playerState = height;
        playerSprite.sprite = stance;
        sfxManager.QueueSound(false, sfxManager.metronome);


        if (playerSettings.parryEngage == PlayerSettings.ParryEngage.Enabled)
        {
            yield return new WaitUntil(() => engageParry.IsPressed()); // Wait's until engage is pressed
        }
        else { 
            inputTiming = enemy.windupValue; 
        }


        if (engageParry.IsPressed())
        {
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
            Debug.Log("ENGAGING");
            inputTiming = enemy.windupValue;
            isEngaging = true;
            yield return new WaitForSeconds(0.3f);
            success = judge.CheckTiming();
            yield return new WaitForSeconds(0.05f);
        }
        else
        {
            //Wait
            yield return new WaitForSeconds(60 / musicManager.metroTempo * parryLengthBeats);
        }

        
        

        //Deactivate Parry
        if (!success)
        {
            ToIdle();
        }
        
    }

    public void ToIdle()
    {
        playerState = State.Idle;
        playerSprite.sprite = idle;
        isEngaging = false;
        success = false;
    }

    public IEnumerator SuccessParry()
    {
        
        var main = musicCircle.main;
        if (playerState == State.Hurting)
            yield return null;

        //Hard coding parry sparks to move vertically based on parry stance (sorry! we can fix this later!)
        //TODO: Trigger music circle only when perfect parry?
        if (playerState == State.ParryHigh)
        {
            playerSprite.sprite = highEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, 0.4f, parrySparks.transform.localPosition.z);
            main.startColor = Color.yellow;
            musicCircle.transform.localPosition = new Vector3(musicCircle.transform.localPosition.x, 0.5f, musicCircle.transform.localPosition.z);
        }
        else if (playerState == State.ParryMedium)
        {
            playerSprite.sprite = medEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, 0f, parrySparks.transform.localPosition.z);
            musicCircle.transform.localPosition = new Vector3(musicCircle.transform.localPosition.x, -0.25f, musicCircle.transform.localPosition.z);
            main.startColor = Color.purple;
        }
        else if (playerState == State.ParryLow)
        {
            playerSprite.sprite = lowEnd;
            parrySparks.transform.localPosition = new Vector3(parrySparks.transform.localPosition.x, -0.4f, parrySparks.transform.localPosition.z);
            musicCircle.transform.localPosition = new Vector3(musicCircle.transform.localPosition.x, -1f, musicCircle.transform.localPosition.z);
            main.startColor = Color.green;
        }
        transform.position = parryPos.position;
        musicCircle.Play();
        parrySparks.Play();
        yield return new WaitForSeconds(60 / musicManager.metroTempo);
        transform.position = defaultPos.position;

        //Deactivate Parry
        ToIdle();
    }
}