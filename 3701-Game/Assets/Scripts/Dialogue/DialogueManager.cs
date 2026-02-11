using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogueJson;
    public GameObject dialogueBox;
    DialogueList dialogueData; //You can find this class in Dialogue.cs
    private enum SpeakerState { PlayerSpeaking, NPCSpeaking };
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

        
        Debug.Log(dialogueData.dialogue[currSpeakerIndex].characterName + ": " + dialogueData.dialogue[currSpeakerIndex].text[currTextIndex]);

        
        CheckSpeakerState();
        //Check who is speaking and decide wether to render the string of dialogue or prompt a dialogue choice from the player

        switch (speakerState) {

            case SpeakerState.NPCSpeaking:

                //render line
                Debug.Log("Render NPC Speak screen");
                break;
            case SpeakerState.PlayerSpeaking:

                //prompt dialogue choice
                Debug.Log("Render Player Speak screen");
                break;
        }


    }

    public void CheckSpeakerState()
    {
        if (dialogueData.dialogue[currSpeakerIndex].characterName == "Player") speakerState = SpeakerState.PlayerSpeaking;
        else speakerState = SpeakerState.NPCSpeaking;

    }
    public void RenderNextDialogue()
    {

        if (IsThereText())
        {
            RenderDialogue();
            currTextIndex++;
        }
        else if (IsThereDialogue()) //no more text to render in this particular instance, move to next speaker instance
        {
            currSpeakerIndex++;
            ResetTextIndex(); //reset text index

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
