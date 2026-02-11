using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogueJson;
    public GameObject dialogueBox;
    public GameObject dialoguePrefab;
    DialogueList dialogueData; //You can find this class in Dialogue.cs
    private enum SpeakerState { Speaking, Decision };
    private SpeakerState speakerState;
    private int currSpeakerIndex;
    private int currTextIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSpeakerIndex();
        ResetTextIndex();
        LoadJsonFile();


    }

    void Update()
    {
        HandleInput();

    }
    public void LoadJsonFile()
    {
        //get file path from Json file from TextAsset in inspector
        string filePath = dialogueJson.text;

        //apply all Json items into our data container
        dialogueData = JsonUtility.FromJson<DialogueList>(filePath);


    }

    public void HandleInput()
    {
        //Gotta handle input based on whether we're waiting for the player to respond or laying down lines
        if (Input.GetMouseButtonUp(0))
        {
            if (IsThereDialogue()) // typical we will start with an NPC talking to you
            {
                RenderNextDialogue();
            }
            else if (!IsThereDialogue())
            {
                Debug.Log("No more dialogue");
            }
        }
    
    }

    public void LoadNewDialogue(TextAsset textAsset)
    {
        dialogueJson = textAsset;
        LoadJsonFile();
    }

    public void RenderDialogue()
    {

        string speakerName = dialogueData.dialogue[currSpeakerIndex].characterName;
        string text = dialogueData.dialogue[currSpeakerIndex].text[currTextIndex];
        Debug.Log(dialogueData.dialogue[currSpeakerIndex].characterName + ": " + dialogueData.dialogue[currSpeakerIndex].text[currTextIndex]);


        CheckSpeakerState();
        //Check who is speaking and decide wether to render the string of dialogue or prompt a dialogue choice from the player

        GameObject newDialogue = Instantiate(dialoguePrefab, dialogueBox.transform);

        newDialogue.GetComponent<DialogueObject>().SetText(speakerName, text);
        switch (speakerState) {

            case SpeakerState.Speaking:

            
                Debug.Log("Render NPC Speak screen");
                break;
            case SpeakerState.Decision:

              
                //prompt dialogue choice
                Debug.Log("Render Player Speak screen");
                break;
        }


    }

    public void CheckSpeakerState()
    {
        if (dialogueData.dialogue[currSpeakerIndex].decision) speakerState = SpeakerState.Decision;
        else speakerState = SpeakerState.Speaking;

    }
    public void RenderNextDialogue()
    {
        currTextIndex++;
        if (IsThereText())
        {

            RenderDialogue();

        }
        else
            
        {

            currSpeakerIndex++;
            if (IsThereDialogue()) //no more text to render in this particular instance, but we still have speakers move to next speaker instance
            {
                ResetTextIndex(); //reset text index
                RenderDialogue();

            }
           


        }



    }

    public void PlayerDialoguePick()
    {
    
    }

    public bool IsThereDialogue()
    {
        return currSpeakerIndex < dialogueData.dialogue.Length;
    }

    public bool IsThereText()
    {
        return currTextIndex < dialogueData.dialogue[currSpeakerIndex].text.Length;
    }

    public void ResetSpeakerIndex()
    {
        currSpeakerIndex = 0;

    }
    public void ResetTextIndex()
    {
        currTextIndex = 0;
    }
}
