using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("Menu Options")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;
    public bool paused;
    private InputAction pause;
    public void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
    }

    public void Update()
    {
        if (pause.WasPressedThisFrame())
        {
            PauseUnpause();
        }
    }
    public void PauseUnpause()
    {
        paused = true;
        if (settingsPanel.activeInHierarchy)
        {
            settingsPanel.SetActive(false);
            mainPanel.SetActive(true);
        } else if (difficultyPanel.activeInHierarchy)
        {
            difficultyPanel.SetActive(false);
            settingsPanel.SetActive(true);
        } else if (gameplayPanel.activeInHierarchy)
        {
            gameplayPanel.SetActive(false);
            settingsPanel.SetActive(true);
        } else if (audioPanel.activeInHierarchy)
        {
            audioPanel.SetActive(false);
            settingsPanel.SetActive(true);
        } else if (controlsPanel.activeInHierarchy)
        {
            controlsPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
        else if (mainPanel.activeInHierarchy)
        {
            Resume();
        } else
        {
        mainPanel.SetActive(true);
        Time.timeScale = 0;
        }
    }

    // Resumes game and closes pause menu
    public void Resume()
    {
        paused = false;
        mainPanel.SetActive(false);
        Time.timeScale = 1;
    }

    // Switches from main panel to settings panel
    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // Returns from settings panel to main panel
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    // Exits to  HUB scene
    public void ExitToHub()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("HubNavigationTest");
    }

    // Exits to title screen scene
    public void ExitToTitleScreen()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
}