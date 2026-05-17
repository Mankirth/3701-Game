using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    public PlayerSettings settings;

    public TMP_Text highKey, medKey, lowKey, engageKey;
    public Slider masterVolume, sfxVolume;

    public Toggle engageToggle, healToggle, iconToggle, outlineToggle, engageIconToggle;

    public int presetIndex;
    public TMP_Text controlPresetTMP, difficultyText;

    [SerializeField]
    private InputIconsDB inputIcons;

    private InputActionRebindingExtensions.RebindingOperation currentRebind;

    public void Start()
    {
        outlineToggle.isOn = settings.GetOutlineState();
        iconToggle.isOn = settings.GetIcon();
        engageToggle.isOn = settings.GetParryEngage();
        healToggle.isOn = settings.GetHeal();
        engageIconToggle.isOn = settings.GetEngageParryIcon();
        controlPresetTMP.text = settings.controls.ToString();

        highKey.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
        medKey.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
        lowKey.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
        engageKey.text = InputSystem.actions.FindAction("EngageParry").GetBindingDisplayString();
        masterVolume.value = settings.GetMasterVolume();
        sfxVolume.value = settings.GetVolume();

    }

    public void Update()
    {
        outlineToggle.isOn = settings.GetOutlineState();
        iconToggle.isOn = settings.GetIcon();
        engageToggle.isOn = settings.GetParryEngage();
        healToggle.isOn = settings.GetHeal();
        engageIconToggle.isOn = settings.GetEngageParryIcon();
        controlPresetTMP.text = settings.controls.ToString();
    }
    public enum Stance
    {
        ParryHigh,
        ParryMedium,
        ParryLow,
        EngageParry,
    }

    public void ChangeVolume(int type)
    {
        if (type == 0)
        {
            Debug.Log(masterVolume.value);
            settings.UpdateMasterVolume(masterVolume.value);
        }
        else
        {
            Debug.Log(sfxVolume.value);
            settings.ChangeVolume(sfxVolume.value);
        }
    }


    public void NextControlPreset()
    {
        if (presetIndex < 3)
        {
            presetIndex++;
        }
        else
        {
            presetIndex = 0;
        }
        settings.SetControls((PlayerSettings.Controls)presetIndex);
        controlPresetTMP.text = settings.controls.ToString();

        if (settings.controls.ToString() != "Controller")
        {
            highKey.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
            medKey.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
            lowKey.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
            engageKey.text = InputSystem.actions.FindAction("EngageParry").GetBindingDisplayString();
        }
        else
        {
            highKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryHigh").bindings[0].effectivePath];
            medKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryMedium").bindings[0].effectivePath];
            lowKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryLow").bindings[0].effectivePath]; 
            engageKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("EngageParry").bindings[0].effectivePath];
        }

        Debug.Log("Settings: " + settings.controls.ToString());
    }

    public void PrevControlPreset()
    {
        
        if (presetIndex > 0) {
            presetIndex--;
        }
        else
        {
            presetIndex = 3;
        }
        settings.SetControls((PlayerSettings.Controls)presetIndex);
        controlPresetTMP.text = settings.controls.ToString();
        if (settings.controls.ToString() != "Controller")
        {
            highKey.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
            medKey.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
            lowKey.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
            engageKey.text = InputSystem.actions.FindAction("EngageParry").GetBindingDisplayString();
        }
        else
        {
            highKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryHigh").bindings[0].effectivePath];
            medKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryMedium").bindings[0].effectivePath];
            lowKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryLow").bindings[0].effectivePath]; 
            engageKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("EngageParry").bindings[0].effectivePath];
        }
        Debug.Log("Settings: " + settings.controls.ToString());
    }

    public void ToggleEngage()
    {
        if (engageToggle.isOn)
        {
            settings.parryEngage = PlayerSettings.ParryEngage.Enabled;
        }
        else
        {
            settings.parryEngage = PlayerSettings.ParryEngage.Disabled;
        }
    }

    public void ToggleHeal()
    {
        if (healToggle.isOn)
        {
            settings.healOnGood = PlayerSettings.HealOnGood.Enabled;
        }
        else
        {
            settings.healOnGood = PlayerSettings.HealOnGood.Disabled;
        }
    }

    public void ToggleIcon()
    {
        if (iconToggle.isOn)
        {
            settings.inputIcon = PlayerSettings.InputIcon.Show;
        }
        else
        {
            settings.inputIcon = PlayerSettings.InputIcon.Hide;
        }
    }

    public void ToggleOutline()
    {
        if (outlineToggle.isOn)
        {
            settings.outline = PlayerSettings.Outline.Default;
        }
        else
        {
            settings.outline = PlayerSettings.Outline.Fading;
        }
    }

    public void ToggleEngageParryIcon()
    {
        if (engageIconToggle.isOn)
        {
            settings.engageIcon = PlayerSettings.EngageIcon.Enabled;
        }
        else
        {
            settings.engageIcon = PlayerSettings.EngageIcon.Disabled;
        }
    }

    public void ChangeDifficulty(int index)
    {
        settings.difficulty = (PlayerSettings.Difficulty)index;
        settings.SetDifficultyPreset(settings.difficulty);
        difficultyText.text = "Current Difficulty: " + settings.difficulty.ToString();
    }

    public void ChangeKeyBind(int index)
    {

        if (currentRebind != null)
        {
            currentRebind.Cancel();
            currentRebind.Dispose();
            currentRebind = null;
        }

        Stance stance = (Stance)index;
        InputAction rebindInput = InputSystem.actions.FindAction(stance.ToString());
        StartCoroutine(ChangeBinding(rebindInput, stance));
    }

    public IEnumerator ChangeBinding(InputAction action, Stance stance)
    {
        action.Disable();

        var rebindOp = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("Escape")
            .OnComplete(operation =>
            {
                action.Enable();

                string key = action.GetBindingDisplayString();

                Debug.Log("Rebind complete: " + key);

                switch (stance)
                {
                    case Stance.ParryHigh:
                        if (settings.controls.ToString() != "Controller")
                        {
                            highKey.SetText(key);
                        }  
                        else 
                        {
                            highKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryHigh").bindings[0].effectivePath];
                        }
                        break;
                    case Stance.ParryMedium:
                        if (settings.controls.ToString() != "Controller")
                        {
                            medKey.SetText(key);
                        }
                        else
                        {
                            medKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryMedium").bindings[0].effectivePath];
                        }
                        break;
                    case Stance.ParryLow:
                        if (settings.controls.ToString() != "Controller")
                        {
                            lowKey.SetText(key);
                        }
                        else
                        {
                            lowKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("ParryLow").bindings[0].effectivePath];
                        }
                        break;
                    case Stance.EngageParry:
                        if (settings.controls.ToString() != "Controller")
                        {
                            engageKey.SetText(key);
                        }
                        else
                        {
                            engageKey.text = inputIcons.lookupDict[InputSystem.actions.FindAction("EngageParry").bindings[0].effectivePath];
                        }
                        break;
                }

                operation.Dispose();
            })
            .Start();

        yield return null;
    }

    public void SetSelected(GameObject btn)
    {
        EventSystem.current.SetSelectedGameObject(btn);
    }


}
