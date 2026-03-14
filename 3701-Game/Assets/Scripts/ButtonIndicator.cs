using UnityEngine;

public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject highKey, medKey, lowKey, engageKey;



    public void ShowKey(State beatStance)
    {
        if (beatStance == State.ParryLow)
        { 
           lowKey.SetActive(true);
        }
        else if (beatStance == State.ParryMedium)
        {

            medKey.SetActive(true);
        }
        else if (beatStance == State.ParryHigh)
        {
            highKey.SetActive(true);
        }

    }
    
    public void ShowEngageKey()
    {
        engageKey.SetActive(true);
    }

    public void HideKey()
    {
        highKey.SetActive(false);
        medKey.SetActive(false);
        lowKey.SetActive(false);
        engageKey.SetActive(false);
    }
}
