using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public PlayerSettings settings;

    public TMP_Text highKey, medKey, lowKey;
    public Slider masterVolume, sfxVolume;

    public Toggle engageToggle, iconToggle;
    public void Start()
    {
        masterVolume.value = settings.GetMasterVolume();
        sfxVolume.value = settings.GetVolume();
        
    }
    public enum Stance
    {
        ParryHigh,
        ParryMedium,
        ParryLow,
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

    public void ChangeKeyBind(int index)
    {
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
                        highKey.SetText(key);
                        break;
                }

                operation.Dispose();
            })
            .Start();

        yield return null;
    }

}
