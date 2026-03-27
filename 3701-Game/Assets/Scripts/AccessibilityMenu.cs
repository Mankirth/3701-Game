using UnityEngine;
using UnityEngine.UI;

public class AccessibilityMenu : MonoBehaviour
{
    public PlayerSettings settings;

    public Slider rLow, gLow, bLow, rMed, gMed, bMed, rHigh, gHigh, bHigh;

    public Image lowOutline, medOutline, highOutline;

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

        highOutline.color = settings.highColor;
        medOutline.color = settings.medColor;
        lowOutline.color = settings.lowColor;
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
}
