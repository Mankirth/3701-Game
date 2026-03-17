using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuController pauseMenuController;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private GameManager gameManager;
    public TMP_Text currentDifficultyText;
    // [SerializeField] private SfxManager sfxManager;

    [Header("Control Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

    private void Awake()
    {
        UpdateDifficultyText();
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
    private void UpdateDifficultyText()
    {
        // if (playerSettings == null)
        // {
        //     Debug.Log("SettingsMenuController: PlayerSettings is null, need to assign in Unity Inspector");
        //     if (currentDifficultyText != null)
        //     {
        //         currentDifficultyText.text = "Missing";
        //     }
        //     return;
        // }

        // if (currentDifficultyText == null)
        // {
        //     Debug.Log("SettingsMenuController: Current Difficulty Text is null, need to assign in Unity Inspector");
        //     return;
        // }

        currentDifficultyText.text = "Current Difficulty: " + playerSettings.difficulty.ToString();
    }

    public void SetDifficulty(int diff)
    {
        // if (playerSettings == null)
        // {
        //     Debug.Log("SettingsMenuController: PlayerSettings is null, need to assign in Unity Inspector");
        //     return;
        // }

        playerSettings.SetDifficultyPreset((PlayerSettings.Difficulty)diff);

        if (gameManager != null)
        {
            gameManager.ChangeDifficulty(diff);
        }

        UpdateDifficultyText();
    }

    // Toggle J parry engage requirement on or off (is this still an option?)

    // Music slider

    // SFX Slider

}