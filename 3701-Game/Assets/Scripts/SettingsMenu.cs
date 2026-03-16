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

    public IEnumerator ChangeBinding(InputAction rebindInput, Stance stance)
    {
        String key = "";
        

        InputSystem.onAnyButtonPress.CallOnce(ctrl => key = ctrl.displayName);
        yield return new WaitUntil(() => Keyboard.current.anyKey.wasPressedThisFrame);
        InputSystem.actions.FindAction(stance.ToString()).Disable();
        rebindInput.PerformInteractiveRebinding()
          .WithControlsExcluding("Mouse")
          .WithControlsExcluding("esc")
          .OnMatchWaitForAnother(0.1f)
          .Start();
        Debug.Log("COMPLETE");
        InputSystem.actions.FindAction(stance.ToString()).Enable();
        switch (stance)
        {
            case (Stance.ParryHigh):
                highKey.SetText(key);
                Debug.Log("Pressed Key: " + key.ToString());
                break;
        }
    }

}
