using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    InputAction pause;
    bool paused;
    public bool pausable = true;
    public GameObject hud, pauseMenu, winMenu, loseMenu;
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
        pauseMenu.SetActive(paused);
        hud.SetActive(!paused);
        Debug.Log(paused);
        if (paused)
            musicManager.musicPlayEvent.setPaused(true);
        else
            musicManager.musicPlayEvent.setPaused(false);
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
        pausable = false;
        hud.SetActive(false);
        musicManager.musicPlayEvent.setPaused(true);
        if (!win)
        {
            loseMenu.SetActive(true);
        }
        else
            winMenu.SetActive(true);
    }


}
