using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuController pauseMenuController;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private SfxManager sfxManager;

    [Header("Control Panels")]
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Difficulty")]
    [SerializeField] private Toggle parryEngageToggle;
    [SerializeField] private Toggle keyIndicatorsToggle;
    [SerializeField] private Toggle fadingOutlineToggle;

    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MusicBusPath = "bus:/Music";

    private void Awake()
    {
        SyncTogglesToSettings();
        SyncSlidersToSettings();
    }

    // -------------------------------------------------------------------------
    // Control Panels
    // -------------------------------------------------------------------------

    // Shows difficulty panel and hides the others
    public void ShowDifficulty()
    {
        SetActivePanel(difficultyPanel);
    }

    // Shows audio panel and hides the others
    public void ShowAudio()
    {
        SetActivePanel(audioPanel);
    }

    // Shows controls panel and hides the others
    public void ShowControls()
    {
        SetActivePanel(controlsPanel);
    }

    // Closes settings panel and returns to main pause panel
    public void Back()
    {
        pauseMenuController.CloseSettings();
    }

    private void SetActivePanel(GameObject targetPanel)
    {
        difficultyPanel.SetActive(difficultyPanel == targetPanel);
        audioPanel.SetActive(audioPanel == targetPanel);
        controlsPanel.SetActive(controlsPanel == targetPanel);
    }

    // -------------------------------------------------------------------------
    // Difficulty
    // -------------------------------------------------------------------------

    // Applies difficulty preset 0 = Easy, 1 = Normal, 2 = Hard
    public void SetDifficultyPreset(int preset)
    {
        playerSettings.SetDifficultyPreset((PlayerSettings.Difficulty)preset);
        SyncTogglesToSettings();
    }

    // Toggles J parry engage requirement on or off
    public void SetParryEngageToggle(bool value)
    {
        playerSettings.parryEngage = value
            ? PlayerSettings.ParryEngage.Enabled
            : PlayerSettings.ParryEngage.Disabled;
        playerSettings.difficulty = PlayerSettings.Difficulty.Custom;
    }

    // Toggles key indicators (input icons) on or off
    public void SetKeyIndicatorsToggle(bool value)
    {
        playerSettings.inputIcon = value
            ? PlayerSettings.InputIcon.Show
            : PlayerSettings.InputIcon.Hide;
        playerSettings.difficulty = PlayerSettings.Difficulty.Custom;
    }

    // Toggles the fading outline on or off
    public void SetFadingOutlineToggle(bool value)
    {
        playerSettings.outline = value
            ? PlayerSettings.Outline.Fading
            : PlayerSettings.Outline.Default;
        playerSettings.difficulty = PlayerSettings.Difficulty.Custom;
    }

    // -------------------------------------------------------------------------
    // Audio
    // -------------------------------------------------------------------------

    // Sets the music bus volume via FMOD (0–1)
    public void SetMusicVolume(float value)
    {
        RuntimeManager.GetBus(MusicBusPath).setVolume(value);
    }

    // Sets the SFX AudioSource volumes (0–1)
    public void SetSFXVolume(float value)
    {
        sfxManager.SetVolume(value);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private void SyncTogglesToSettings()
    {
        parryEngageToggle.SetIsOnWithoutNotify(
            playerSettings.parryEngage == PlayerSettings.ParryEngage.Enabled);

        keyIndicatorsToggle.SetIsOnWithoutNotify(
            playerSettings.inputIcon == PlayerSettings.InputIcon.Show);

        fadingOutlineToggle.SetIsOnWithoutNotify(
            playerSettings.outline == PlayerSettings.Outline.Fading);
    }

    private void SyncSlidersToSettings()
    {
        musicSlider.SetValueWithoutNotify(1f);
        sfxSlider.SetValueWithoutNotify(1f);
    }
}