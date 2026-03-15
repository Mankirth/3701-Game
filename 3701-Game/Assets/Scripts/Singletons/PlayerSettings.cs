using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public Difficulty difficulty = Difficulty.Normal;

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
    public ColorblindMode colorBlind = ColorblindMode.Off; 
    public enum ColorblindMode
    {
        Off,
        RedGreen,
        BluePurple,
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
        controls = Controls.Default;
        parryEngage = ParryEngage.Enabled;
        outline = Outline.Default;
        colorBlind = ColorblindMode.Off;
        metronomeSFX = MetronomeSFX.Normal;
        stanceSFX = StanceSFX.Normal;
        inputIcon = InputIcon.Hide;
        gameSpeed = GameSpeed.Normal;
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
