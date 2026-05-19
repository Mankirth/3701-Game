using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject highKey, medKey, lowKey, engageKey;
    public TMP_Text highKeyText, medKeyText, lowKeyText, engageKeyText;

    private SpriteRenderer highKeyBg, medKeyBg, lowKeyBg, engageKeyBg;

    public InputIconsDB inputIcons;

    [SerializeField]
    private PlayerSettings settings;
    public void Start()
    {
        if (settings.controls.ToString() != "Controller")
        {
            highKeyText.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
            medKeyText.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
            lowKeyText.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
            engageKeyText.text = InputSystem.actions.FindAction("EngageParry").GetBindingDisplayString();
        }
        else
        {
            highKeyText.text = inputIcons?.lookupDict[InputSystem.actions.FindAction("ParryHigh").bindings[0].effectivePath];
            medKeyText.text = inputIcons?.lookupDict[InputSystem.actions.FindAction("ParryMedium").bindings[0].effectivePath];
            lowKeyText.text = inputIcons?.lookupDict[InputSystem.actions.FindAction("ParryLow").bindings[0].effectivePath];
            engageKeyText.text = inputIcons?.lookupDict[InputSystem.actions.FindAction("EngageParry").bindings[0].effectivePath];
        }

        highKeyBg = highKey.GetComponent<SpriteRenderer>();
        medKeyBg = medKey.GetComponent<SpriteRenderer>();
        lowKeyBg = lowKey.GetComponent<SpriteRenderer>();
        engageKeyBg = engageKey.GetComponent<SpriteRenderer>();
    }

    public void ShowKey(State beatStance, bool onController)
    {
        if (onController)
        {
            highKeyBg.color = new Color(1, 1, 1, 0);
            medKeyBg.color = new Color(1, 1, 1, 0);
            lowKeyBg.color = new Color(1, 1, 1, 0);
            engageKeyBg.color = new Color(1, 1, 1, 0);
        }
        else
        {
            highKeyBg.color = new Color(1, 1, 1, 1);
            medKeyBg.color = new Color(1, 1, 1, 1);
            lowKeyBg.color = new Color(1, 1, 1, 1);
            engageKeyBg.color = new Color(1, 1, 1, 1);
        }
        if (beatStance == State.ParryLow)
        {
            lowKey.SetActive(true);
            lowKeyText.text = !onController ? InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString() : inputIcons.lookupDict[InputSystem.actions.FindAction("ParryLow").bindings[0].effectivePath];
        }
        else if (beatStance == State.ParryMedium)
        {
            medKey.SetActive(true);
            medKeyText.text = !onController ? InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString() : inputIcons.lookupDict[InputSystem.actions.FindAction("ParryMedium").bindings[0].effectivePath];
        }
        else if (beatStance == State.ParryHigh)
        {
            highKey.SetActive(true);
            highKeyText.text = !onController ? InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString() : inputIcons.lookupDict[InputSystem.actions.FindAction("ParryHigh").bindings[0].effectivePath]; 
        }

    }

    public void ShowEngageKey(bool onController)
    {
        engageKeyText.text = !onController ? InputSystem.actions.FindAction("EngageParry").GetBindingDisplayString() : inputIcons.lookupDict[InputSystem.actions.FindAction("EngageParry").bindings[0].effectivePath];
        engageKey.SetActive(true);
    }

    public void HideEngageKey()
    {
        engageKey.SetActive(false);
    }
    public void HideKey()
    {
        highKey.SetActive(false);
        medKey.SetActive(false);
        lowKey.SetActive(false);
    }
}
