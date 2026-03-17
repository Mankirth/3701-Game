using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuController pauseMenuController;
    // [SerializeField] private PlayerSettings playerSettings;
    // [SerializeField] private SfxManager sfxManager;

    [Header("Control Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

    private void Awake()
    {
    
    }

    public void ShowDifficulty()
    {
        difficultyPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ShowAudio()
    {
        audioPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void HideDifficulty()
    {
        difficultyPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void HideAudio()
    {
        audioPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    // Closes settings panel and returns to main (pause) panel
    public void BackToMain()
    {
        pauseMenuController.CloseSettings();

    }

    // Set difficulty

    // Toggle J parry engage requirement on or off (is this still an option?)

    // Toggle key indicators (input icons) on or off

    // Toggle fading outline on or off

    // Music slider

    // SFX Slider

}