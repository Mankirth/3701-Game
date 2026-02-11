using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceObject : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public DialogueManager dialogueManager;
    string temp1;
    string temp2;
    public void SetText(string text1, string text2)
    {
        button1.GetComponent<TMP_Text>().text = text1;
        temp1 = text1;
        button2.GetComponent<TMP_Text>().text = text2;
        temp2 = text2;
        
        //find dialogue manager in hierarchy and establish reference
        GameObject manager = GameObject.Find("DialogueManager");
        dialogueManager = manager.GetComponent<DialogueManager>();
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
        dialogueManager.CreateDialogueObject("Player", text);
        Destroy(gameObject);
    }
}
