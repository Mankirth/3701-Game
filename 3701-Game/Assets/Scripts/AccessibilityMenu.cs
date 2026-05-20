using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AccessibilityMenu : MonoBehaviour
{
    public PlayerSettings settings;

    public Slider rLow, gLow, bLow, rMed, gMed, bMed, rHigh, gHigh, bHigh;

    public Image lowOutline, medOutline, highOutline;

    public Toggle parryZoom, dodgeCamera;

    public TMP_Text colorPresetTMP;

    private int presetIndex;

    private void Awake()
    {
        rLow.SetValueWithoutNotify(settings.lowColor.r);
        gLow.SetValueWithoutNotify(settings.lowColor.g);
        bLow.SetValueWithoutNotify(settings.lowColor.b);

        rMed.SetValueWithoutNotify(settings.medColor.r);
        gMed.SetValueWithoutNotify(settings.medColor.g);
        bMed.SetValueWithoutNotify(settings.medColor.b);

        rHigh.SetValueWithoutNotify(settings.highColor.r);
        gHigh.SetValueWithoutNotify(settings.highColor.g);
        bHigh.SetValueWithoutNotify(settings.highColor.b);

        colorPresetTMP.text = settings.controls.ToString();

        highOutline.color = settings.highColor;
        medOutline.color = settings.medColor;
        lowOutline.color = settings.lowColor;

        parryZoom.isOn = settings.GetParryZoom();
        dodgeCamera.isOn = settings.GetDodgeCamera();
    }

    public void ChangeHighColor()
    {
        settings.highColor = new Color(rHigh.value,gHigh.value, bHigh.value);
        highOutline.color = settings.highColor;
    }
    public void ChangeMedColor()
    {
        settings.medColor = new Color(rMed.value, gMed.value, bMed.value);
        medOutline.color = settings.medColor;
    }
    public void ChangeLowColor()
    {
        settings.lowColor = new Color(rLow.value, gLow.value, bLow.value);
        lowOutline.color = settings.lowColor;
    }

    public void ChangeParryZoom()
    {
        if (parryZoom.isOn)
        {
            settings.parryZoom = PlayerSettings.ParryZoom.Enabled;       
        }
        else
        {
            settings.parryZoom = PlayerSettings.ParryZoom.Disabled;
        }
    }

    public void ChangeDodgeCamera()
    {
        if (dodgeCamera.isOn)
        {
            settings.dodgeCamera = PlayerSettings.DodgeCamera.Enabled;
        }
        else
        {
            settings.dodgeCamera = PlayerSettings.DodgeCamera.Disabled;
        }
    }


    public void NextColorPreset()
    {
        if (presetIndex < 4)
        {
            presetIndex++;
        }
        else
        {
            presetIndex = 0;
        }

        settings.SetColorMode((PlayerSettings.ColorMode)presetIndex);
        colorPresetTMP.text = settings.colorMode.ToString();
        Debug.Log("Settings: " + settings.controls.ToString());

        highOutline.color = settings.highColor;
        medOutline.color = settings.medColor;
        lowOutline.color = settings.lowColor;

        rLow.SetValueWithoutNotify(settings.lowColor.r);
        gLow.SetValueWithoutNotify(settings.lowColor.g);
        bLow.SetValueWithoutNotify(settings.lowColor.b);

        rMed.SetValueWithoutNotify(settings.medColor.r);
        gMed.SetValueWithoutNotify(settings.medColor.g);
        bMed.SetValueWithoutNotify(settings.medColor.b);

        rHigh.SetValueWithoutNotify(settings.highColor.r);
        gHigh.SetValueWithoutNotify(settings.highColor.g);
        bHigh.SetValueWithoutNotify(settings.highColor.b);
    }

    public void PrevColorPreset()
    {

        if (presetIndex > 0)
        {
            presetIndex--;
        }
        else
        {
            presetIndex = 4;
        }
        settings.SetColorMode((PlayerSettings.ColorMode)presetIndex);
        colorPresetTMP.text = settings.colorMode.ToString();
        Debug.Log("Settings: " + settings.colorMode.ToString());

        highOutline.color = settings.highColor;
        medOutline.color = settings.medColor;
        lowOutline.color = settings.lowColor;

        rLow.SetValueWithoutNotify(settings.lowColor.r);
        gLow.SetValueWithoutNotify(settings.lowColor.g);
        bLow.SetValueWithoutNotify(settings.lowColor.b);

        rMed.SetValueWithoutNotify(settings.medColor.r);
        gMed.SetValueWithoutNotify(settings.medColor.g);
        bMed.SetValueWithoutNotify(settings.medColor.b);

        rHigh.SetValueWithoutNotify(settings.highColor.r);
        gHigh.SetValueWithoutNotify(settings.highColor.g);
        bHigh.SetValueWithoutNotify(settings.highColor.b);
    }
}
