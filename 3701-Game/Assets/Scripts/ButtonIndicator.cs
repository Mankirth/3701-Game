using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject highKey, medKey, lowKey, engageKey;
    public TMP_Text highKeyText, medKeyText, lowKeyText;
    public void Start()
    {
        highKeyText.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
        medKeyText.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
        lowKeyText.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
    }

    public void ShowKey(State beatStance)
    {
        if (beatStance == State.ParryLow)
        {
            lowKey.SetActive(true);
            lowKeyText.text = InputSystem.actions.FindAction("ParryLow").GetBindingDisplayString();
        }
        else if (beatStance == State.ParryMedium)
        {
            medKey.SetActive(true);
            medKeyText.text = InputSystem.actions.FindAction("ParryMedium").GetBindingDisplayString();
        }
        else if (beatStance == State.ParryHigh)
        {
            highKey.SetActive(true);
            highKeyText.text = InputSystem.actions.FindAction("ParryHigh").GetBindingDisplayString();
        }

    }

    public void ShowEngageKey()
    {
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
