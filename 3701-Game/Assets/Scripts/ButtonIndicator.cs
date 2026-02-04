using UnityEngine;

public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private Sprite  highKey, medKey, lowKey;

    [SerializeField]
    private SpriteRenderer keySprite;


    public void ShowKey(State beatStance)
    {
        keySprite.enabled = true;
        if (beatStance == State.ParryLow)
        {
            
            keySprite.sprite = lowKey;
        }
        else if (beatStance == State.ParryMedium)
        {

            keySprite.sprite = medKey;
        }
        else if (beatStance == State.ParryHigh)
        {

            keySprite.sprite = highKey;
        }

    }

    public void HideKey()
    {
        keySprite.enabled = false;
    }
}
