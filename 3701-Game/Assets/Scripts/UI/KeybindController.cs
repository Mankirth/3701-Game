using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindController : MonoBehaviour
{
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private SettingsMenuController settingsMenu;

    [Header("Rebind Buttons")]
    [SerializeField] private Button highParryRebindBtn;
    [SerializeField] private Button medParryRebindBtn;
    [SerializeField] private Button lowParryRebindBtn;
    [SerializeField] private Button engageRebindBtn;

    [Header("Current Binding Labels")]
    [SerializeField] private TMP_Text highParryLabel;
    [SerializeField] private TMP_Text medParryLabel;
    [SerializeField] private TMP_Text lowParryLabel;
    [SerializeField] private TMP_Text engageLabel;

    private const string ActionParryHigh   = "ParryHigh";
    private const string ActionParryMedium = "ParryMedium";
    private const string ActionParryLow    = "ParryLow";
    private const string ActionEngageParry = "EngageParry";

    private const string PrefsKeyPrefix = "Keybind_";

    private InputActionRebindingExtensions.RebindingOperation activeRebind;

    private void Start()
    {
        LoadAllOverrides();
        RefreshAllLabels();
    }

    // -------------------------------------------------------------------------
    // Rebinding
    // -------------------------------------------------------------------------

    // Begins interactive rebind for given Input System action name
    public void StartRebind(string actionName)
    {
        InputAction action = InputSystem.actions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogWarning($"[KeybindController] Action not found: {actionName}");
            return;
        }

        SetRebindButtonsInteractable(false);

        activeRebind = action
            .PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(op =>
            {
                op.Dispose();
                activeRebind = null;
                playerSettings.controls = PlayerSettings.Controls.Custom;
                RefreshAllLabels();
                SetRebindButtonsInteractable(true);
            })
            .OnCancel(op =>
            {
                op.Dispose();
                activeRebind = null;
                SetRebindButtonsInteractable(true);
            })
            .Start();
    }

    // -------------------------------------------------------------------------
    // Preset controls
    // -------------------------------------------------------------------------

    // Applies selected controls preset (0 = Default, 1 = Alternate, 2 = Custom)
    public void SetControlsPreset(int preset)
    {
        playerSettings.controls = (PlayerSettings.Controls)preset;

        if (preset == (int)PlayerSettings.Controls.Default)
        {
            RemoveAllOverrides();
        }
        // Alternate key layout can be coded here if needed

        RefreshAllLabels();
    }

    // -------------------------------------------------------------------------
    // Apply / Save / Reset
    // -------------------------------------------------------------------------

    // Persists all current binding overrides to PlayerPrefs
    public void ApplyAndSave()
    {
        SaveOverride(ActionParryHigh);
        SaveOverride(ActionParryMedium);
        SaveOverride(ActionParryLow);
        SaveOverride(ActionEngageParry);
        playerSettings.controls = PlayerSettings.Controls.Custom;
    }

    // Clears all binding overrides and resets to default
    public void ResetToDefault()
    {
        RemoveAllOverrides();
        playerSettings.controls = PlayerSettings.Controls.Default;
        RefreshAllLabels();
    }

    // Cancels any active rebind and returns to control panels
    public void Back()
    {
        activeRebind?.Cancel();
        settingsMenu.ShowControls();
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private void SaveOverride(string actionName)
    {
        InputAction action = InputSystem.actions.FindAction(actionName);
        if (action == null) return;
        PlayerPrefs.SetString(PrefsKeyPrefix + actionName, action.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    private void LoadAllOverrides()
    {
        LoadOverride(ActionParryHigh);
        LoadOverride(ActionParryMedium);
        LoadOverride(ActionParryLow);
        LoadOverride(ActionEngageParry);
    }

    private void LoadOverride(string actionName)
    {
        string json = PlayerPrefs.GetString(PrefsKeyPrefix + actionName, "");
        if (string.IsNullOrEmpty(json)) return;

        InputAction action = InputSystem.actions.FindAction(actionName);
        action?.LoadBindingOverridesFromJson(json);
    }

    private void RemoveAllOverrides()
    {
        RemoveOverride(ActionParryHigh);
        RemoveOverride(ActionParryMedium);
        RemoveOverride(ActionParryLow);
        RemoveOverride(ActionEngageParry);

        PlayerPrefs.DeleteKey(PrefsKeyPrefix + ActionParryHigh);
        PlayerPrefs.DeleteKey(PrefsKeyPrefix + ActionParryMedium);
        PlayerPrefs.DeleteKey(PrefsKeyPrefix + ActionParryLow);
        PlayerPrefs.DeleteKey(PrefsKeyPrefix + ActionEngageParry);
    }

    private void RemoveOverride(string actionName)
    {
        InputSystem.actions.FindAction(actionName)?.RemoveAllBindingOverrides();
    }

    private void RefreshAllLabels()
    {
        highParryLabel.text  = GetBindingDisplayName(ActionParryHigh);
        medParryLabel.text   = GetBindingDisplayName(ActionParryMedium);
        lowParryLabel.text   = GetBindingDisplayName(ActionParryLow);
        engageLabel.text     = GetBindingDisplayName(ActionEngageParry);
    }

    private string GetBindingDisplayName(string actionName)
    {
        InputAction action = InputSystem.actions.FindAction(actionName);
        if (action == null || action.bindings.Count == 0) return "—";
        return InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath);
    }

    private void SetRebindButtonsInteractable(bool interactable)
    {
        highParryRebindBtn.interactable  = interactable;
        medParryRebindBtn.interactable   = interactable;
        lowParryRebindBtn.interactable   = interactable;
        engageRebindBtn.interactable     = interactable;
    }
}