using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerChoiceObject : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public DialogueManager dialogueManager;
    string temp1;
    string temp2;
    public void SetText(string text1, string text2)
    {
        
        button1.GetComponentInChildren<TMP_Text>().text = text1;
      
        button2.GetComponentInChildren<TMP_Text>().text = text2;
        
        temp1 = text1;
        temp2 = text2;

      
    }

    public void SelectOption1()
    {
        CreateDialogueObject(temp1);
    }

    public void SelectOption2()
    {
        CreateDialogueObject (temp2);
    }

    public void CreateDialogueObject(string text)
    {
        //find dialogue manager in hierarchy and establish reference
        GameObject obj = GameObject.FindWithTag("DialogueManager");
        dialogueManager = obj.GetComponent<DialogueManager>();
      
        //Reset
        dialogueManager.decisionState = DialogueManager.DecisionState.NotCreated;
        dialogueManager.speakerState = DialogueManager.SpeakerState.Speaking;

        dialogueManager.CreateDialogueObject("Player", text);

        dialogueManager.MoveToNextDialogueObject(); //move to next dialog object
        Destroy(gameObject);
    }
}
