using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public AudioMixer masterSFX;

    public Difficulty difficulty = Difficulty.Normal;

    public FMOD.Studio.Bus master;

    public Color lowColor, medColor, highColor;
    public void OnEnable()
    {
        //master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    // Custom lets you personalize the experience. By default difficulty affects outlines, input icons, engage parry
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Custom,
    }

    public Controls controls = Controls.Default;

    // Try WSD as default or ASD, also allow key rebinding
    public enum Controls
    {
        Default,
        Alternate,
        Custom
    }
    // Can possible switch all enums with 2 values to booleans

    public ParryEngage parryEngage = ParryEngage.Enabled;
    public enum ParryEngage
    {
        Enabled,
        Disabled
    }

    public Outline outline = Outline.Default;
    public enum Outline
    {
        None,
        Default,
        Fading
    }

    // Do more research on this
    public ColorMode colorMode = ColorMode.Default;
    public enum ColorMode
    {
        Default,
        RedGreen,
        BluePurple,
        Monochromacy,
        Custom,
    }

    public MetronomeSFX metronomeSFX = MetronomeSFX.Normal;
    public enum MetronomeSFX
    {
        Off,
        Quiet,
        Normal,
        Loud,
        Custom
    }

    public StanceSFX stanceSFX = StanceSFX.Normal;
    public enum StanceSFX
    {
        Off,
        Quiet,
        Normal,
        Loud,
        Custom
    }

    public bool GetIcon()
    {
        if (inputIcon == InputIcon.Show)
        {
            return true;
        }
        return false;
    }
    public bool GetParryEngage()
    {
        if (parryEngage == ParryEngage.Enabled)
        {
            return true;
        }
        return false;
    }

    public bool GetOutlineState()
    {
        if (outline == Outline.Default)
        {
            return true;
        }
        return false;
    }
    // Change to use one method for both master and SFX
    public float GetVolume()
    {
        masterSFX.GetFloat("sfxVol", out float vol);
        return vol;
    }
    public void ChangeVolume(float newVol)
    {
        masterSFX.SetFloat("sfxVol", newVol);
    }

    public float GetMasterVolume()
    {
        master.getVolume(out float vol);
        return vol;
    }
    public void UpdateMasterVolume(float volume)
    {
        master.setVolume(volume);
        master.getVolume(out float vol);
        Debug.Log("VOLUME: " + vol);
    }

    public InputIcon inputIcon = InputIcon.Hide;
    public enum InputIcon
    {
        Show,
        Hide,
        ShowAtStart,
    }

    public GameSpeed gameSpeed;
    public enum GameSpeed
    {
        Normal,
        Double,
        Custom
    }

    public void ResetToDefault()
    {
        difficulty = Difficulty.Normal;
        
        SetControls(Controls.Default);
        parryEngage = ParryEngage.Enabled;
        outline = Outline.Fading;
        colorMode = ColorMode.Default;
        metronomeSFX = MetronomeSFX.Normal;
        stanceSFX = StanceSFX.Normal;
        inputIcon = InputIcon.Hide;
        gameSpeed = GameSpeed.Normal;
        
    }

    public void SetControls(Controls preset)
    {
        switch (preset)
        {
            case Controls.Default:
                controls = Controls.Default;
                InputSystem.actions.FindAction("ParryHigh").ApplyBindingOverride(0, "<Keyboard>/w");
                InputSystem.actions.FindAction("ParryMedium").ApplyBindingOverride(0, "<Keyboard>/d");
                InputSystem.actions.FindAction("ParryLow").ApplyBindingOverride(0, "<Keyboard>/space");
                break;
            case Controls.Alternate:
                controls = Controls.Alternate;
                InputSystem.actions.FindAction("ParryHigh").ApplyBindingOverride(0, "<Keyboard>/w");
                InputSystem.actions.FindAction("ParryMedium").ApplyBindingOverride(0, "<Keyboard>/d");
                InputSystem.actions.FindAction("ParryLow").ApplyBindingOverride(0, "<Keyboard>/a");
                break;
            case Controls.Custom:
                controls = Controls.Custom;
                break;
        }
    }

    public float SetOutline(float i, float outBeat)
    {
        if (outline == Outline.Fading)
        {
            return Math.Max(0, (outBeat - (i * 1.11f)) / outBeat);
        }
        else
        {
            return 1.0f;
        }
    }

    public void SetColorMode(ColorMode preset)
    {
        switch (preset)
        {
            case ColorMode.Default:
                colorMode = ColorMode.Default;
                lowColor = new Color(0.5424837f, 0.5882353f, 0f);
                medColor = new Color(0.7034597f, 1f, 0f);
                highColor = new Color(0.0f, 0.4423075f, 0.0f);
                break;
            case ColorMode.BluePurple:
                colorMode = ColorMode.BluePurple;
                lowColor = new Color(0.8f, 0.2f, 0.2f);
                medColor = new Color(0.2f, 0.7f, 0.2f);
                highColor = new Color(1.0f, 0.5f, 0.8f);
                break;
            case ColorMode.RedGreen:
                colorMode = ColorMode.RedGreen;
                lowColor = new Color(0.0f, 0.6f, 0.8f); 
                medColor = new Color(0.8f, 0.4f, 0.0f);
                highColor = new Color(0.7f, 0.2f, 0.8f); 
                break;
            case ColorMode.Monochromacy:
                colorMode = ColorMode.Monochromacy;
                lowColor = new Color(0.2f, 0.2f, 0.2f);
                medColor = new Color(0.5f, 0.5f, 0.5f);
                highColor = new Color(0.9f, 0.9f, 0.9f);
                break;
            case ColorMode.Custom:
                colorMode = ColorMode.Custom;
                break;
        }
    }
    public void SetDifficultyPreset(Difficulty diff)
    {
        switch (diff)
        {
            case Difficulty.Normal:
                ResetToDefault();
                break;
            case Difficulty.Easy:
                difficulty = Difficulty.Easy;
                parryEngage = ParryEngage.Disabled;
                outline = Outline.Default;
                inputIcon = InputIcon.Show;
                break;
            case Difficulty.Hard:
                difficulty = Difficulty.Hard;
                parryEngage = ParryEngage.Enabled;
                outline = Outline.Fading;
                inputIcon = InputIcon.Hide;
                break;

        }

    }


    // Add more accesibility options, something audio related? For blind people? (Make distinct wind up audio cues)
    // Text-to-speech for menu text (a little much)


}
