using TMPro;
using UnityEngine;
using static UnityEditor.Rendering.MaterialUpgrader;

public class PlayerDialogueObject : MonoBehaviour
{
    public TMP_Text dialogueText;
    public RectTransform rectTransform;

    public float maxWidth = 320f;
    public float minWidth = 50F;

 
    public void SetText(string text)
    {
     
        dialogueText.text = $"{text}";

        // Get the width the text would prefer to be
        float preferredWidth = dialogueText.preferredWidth;

        // Clamp the preferred width between min and max
        float newWidth = Mathf.Clamp(preferredWidth, minWidth, maxWidth);

        // Set the RectTransform's size (keeping the current height)
        rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y);
    }
}
