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
    public GameObject hud, pauseMenu, winMenu, loseMenu, phaseMenu;
    public MusicManager musicManager;
    public PhaseManager phaseManager;

    public Health playerHealth;

    [SerializeField]
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.WasPressedThisFrame()){
            PauseUnpause();
        }
        if (musicManager.PhaseChange() == true)
        {
            ChangePhase();
        }
    }

    public void PauseUnpause()
    {
        if (!pausable)
            return;
        paused = !paused;
        pauseMenu.SetActive(paused);
        hud.SetActive(!paused);
        Debug.Log(paused);
        if (paused) { 
            musicManager.musicPlayEvent.setPaused(true);
        phaseMenu.SetActive(false);
    }
        else {
            musicManager.musicPlayEvent.setPaused(false);
            phaseMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string name)
    {
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
            loseMenu.SetActive(true);
        }
        else
            winMenu.SetActive(true);
    }

    public void ChangePhase()
    {
        if (!phaseManager.changingPhase)
        {
            phaseManager.changingPhase = true;
            phaseMenu.SetActive(true);
            StartCoroutine(phaseManager.ShowPhase());
            
        }

    }


}
