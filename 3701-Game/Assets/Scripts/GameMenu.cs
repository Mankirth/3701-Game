using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    InputAction pause;
    bool paused;
    public bool pausable = true;
    public bool changingPhases = false;
    public bool gameOver = false;
    public GameObject hud, pauseMenu, winMenu, loseMenu, phaseMenu, decisionMenu, strikeMenu;
    public MusicManager musicManager;
    private int introTime;
    public PhaseManager phaseManager;

    public Health playerHealth;
    public RelationshipManager relationManager;

    [SerializeField]
    private GameManager gameManager;

    public Animation decisionAnim, strikeanim;
    public Animator hudAnim;
    [SerializeField]
    private Animator promptAnim;

    public bool tutorialEnabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
         hudAnim.keepAnimatorStateOnDisable = true;
        promptAnim.keepAnimatorStateOnDisable = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.WasPressedThisFrame()){
            PauseUnpause();
        }
        if (musicManager != null && musicManager.PhaseChange() == true)
        {
            ChangePhase();
        }

        if (musicManager.SongEnd())
        {
            StrikeHUD();
        }
        musicManager.musicPlayEvent.getTimelinePosition(out introTime);

        if (introTime < 8000 || tutorialEnabled) // Every beatmap starts at 8 seconds (8000ms)
        {
            pausable = false;
        }
        else
        {
            pausable = true;
        }
    }

    public void PauseUnpause()
    {
        if (!pausable)
            return;
        
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        pauseMenu.SetActive(paused);
        hud.SetActive(!paused);
        Debug.Log(paused);
        
        if (paused) { 
            musicManager.musicPlayEvent.setPaused(true);
        }
        else {
            musicManager.musicPlayEvent.setPaused(false);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

    public void EndGame(bool win)
    {
        gameManager.CalculateFinalScore(playerHealth.dodges);
        pausable = false;
        hud.SetActive(false);
        phaseMenu.SetActive(false);
        musicManager.musicPlayEvent.setPaused(true);
        
        if (!win)
        {
            gameOver = true;
            loseMenu.SetActive(true);
        }
        else
        {
            decisionMenu.SetActive(true);
        }
    }

    public void StrikeHUD()
    {
        hudAnim.SetBool("GameOver", true);
        strikeMenu.SetActive(true);
        strikeanim.Play();
    }

    public void ChangePhase()
    {
        if (!phaseManager.changingPhase)
        {
            phaseManager.changingPhase = true;
            phaseMenu.SetActive(true);
            pausable = false;
            StartCoroutine(phaseManager.ShowPhase());
            
        }
        else
        {
            pausable = true;
        }

    }

    public void KillEnemy()
    {
        relationManager.IncreaseNotoriety(gameManager.notorietyVal, gameManager.RPVal);
        decisionMenu.SetActive(false);
        winMenu.SetActive(true);
    }

    public void SpareEnemy()
    {
        relationManager.DecreaseNotoriety(gameManager.notorietyVal, gameManager.RPVal);
        decisionMenu.SetActive(false);
        winMenu.SetActive(true);
    }

}
