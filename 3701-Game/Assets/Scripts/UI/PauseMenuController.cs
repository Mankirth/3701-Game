using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Menu Options")]
    [SerializeField] private GameMenu gameMenu;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    // Resumes game and closes pause menu
    public void Resume()
    {
        gameMenu.PauseUnpause();
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
        gameMenu.LoadScene("HubNavigationTest");
    }

    // Exits to title screen scene
    public void ExitToTitleScreen()
    {
        Time.timeScale = 1;
        gameMenu.LoadScene("TitleScreen");
    }
}