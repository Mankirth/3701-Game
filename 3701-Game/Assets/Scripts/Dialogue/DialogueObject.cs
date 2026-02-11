using UnityEngine;
using TMPro;

public class DialogueObject : MonoBehaviour
{
    TMP_Text dialogueText;

 
    public void SetText(string speakerName, string text)
    {
        dialogueText = GetComponent<TMP_Text>(); //reference its dialogue object first
        dialogueText.text = $"<b>{speakerName}:</b> {text}";
    }
}
