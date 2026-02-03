using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    InputAction pause;
    bool paused, pausable = true;
    public GameObject pauseMenu, winMenu, loseMenu;
    public MusicManager musicManager;
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
    }

    public void PauseUnpause()
    {
        if(!pausable)
            return;
        paused = !paused;
        Time.timeScale = paused ? 0:1;
        pauseMenu.SetActive(paused);
        if(paused)
            musicManager.musicPlayEvent.setPaused(true);
        else
            musicManager.musicPlayEvent.setPaused(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        //UNCOMMENT WHEN BRANCHES ARE MERGED
        //SceneManager.LoadScene("MainMenu");
    }

    public void EndGame(bool win)
    {
        Time.timeScale = 0;
        pausable = false;
        if (!win)
        {
            loseMenu.SetActive(true);
        }
        else
            winMenu.SetActive(true);
    }
}
