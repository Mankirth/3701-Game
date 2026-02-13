using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogueJson;

    public GameObject dialogueBox;
    public GameObject dialoguePrefab;
    public GameObject decisionPrefab;
    public GameObject playerDialoguePrefab;

    public DialogueList dialogueData; //You can find this class in Dialogue.cs
    public enum SpeakerState { Speaking, Decision, Finish};
    public SpeakerState speakerState;

    public enum DecisionState { NotCreated, Waiting };
    public DecisionState decisionState;

    private int currSpeakerIndex;
    private int currTextIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSpeakerIndex();
        ResetTextIndex();
        LoadJsonFile();
        decisionState = DecisionState.NotCreated; //start off as waiting because no dialogue option has been chosen
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        
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
       
            CheckSpeakerState();

            switch (speakerState)
            {
                //render dialogue lines
                case SpeakerState.Speaking:
                    RenderDialogueHandler();
                    break;

                case SpeakerState.Decision:
                    //handle decision
                    
                    if (decisionState == DecisionState.NotCreated) //First time rendering player decision
                    {

                        CreateDecisionObject(dialogueData.dialogue[currSpeakerIndex].text[0],
                            dialogueData.dialogue[currSpeakerIndex].text[1],
                                dialogueData.dialogue[currSpeakerIndex].targetIndex[0],
                                    dialogueData.dialogue[currSpeakerIndex].targetIndex[1]);
                    } 
              
                    break;
            case SpeakerState.Finish:
                //EXIT DIALOGUE
                break;
            }

        

    }

    public void LoadNewDialogue(TextAsset textAsset)
    {
        dialogueJson = textAsset;
        LoadJsonFile();
    }

 

    public void RenderDialogue()
    {

       
        string text = dialogueData.dialogue[currSpeakerIndex].text[currTextIndex];
        string speaker = dialogueData.dialogue[currSpeakerIndex].characterName;

        if (speaker != "Fencer")
        {

            CreateDialogueObject(text);
        } else
        {
            CreatePlayerDialogueObject(text);
          
        }

      



    }

    public void CheckSpeakerState()
    {
        if (!IsThereDialogue() || dialogueData.dialogue[currSpeakerIndex].endDialogue)  speakerState = SpeakerState.Finish; //we have no more dialogue objects to get through or abrupt end
        else if (dialogueData.dialogue[currSpeakerIndex].hasDecision) speakerState = SpeakerState.Decision; //we are waiting on a decision
        else speakerState = SpeakerState.Speaking;

    }
    public void RenderDialogueHandler()
    {

            //check if we have lines to render
            if (IsThereText())
            {
            
                RenderDialogue();    //render current line
            
               
              currTextIndex++; //increment text line
            }

        //No more lines to render
        //check if we are on the correct target dialogue (i.e: just coming out of a player choice and this is post-response)
        else if    
                (dialogueData.dialogue[currSpeakerIndex].targetIndex.Length > 0) //typically first line not an actual pointer
            {
                MoveToTargetDialogueObject(dialogueData.dialogue[currSpeakerIndex].targetIndex[0]); //move to target line and exit choice tree
                HandleInput(); //run it back
        }
        else
        {

            MoveToNextDialogueObject(); //move on to next potential dialogue object
            HandleInput();  //run it back

        }
        

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

    void ResetTextIndex()
    {
        currTextIndex = 0;  
    }

    public void CreateDialogueObject(string text)
    {
      
        GameObject newDialogue = Instantiate(dialoguePrefab, dialogueBox.transform);

        newDialogue.GetComponent<DialogueObject>().SetText(text);
    }

    public void CreateDecisionObject(string text1, string text2, int target1, int target2)
    {
        decisionState = DecisionState.Waiting; //created, waiting for player response

        GameObject newDecision = Instantiate(decisionPrefab, dialogueBox.transform);
        newDecision.GetComponent<PlayerChoiceObject>().SetText(text1, text2, target1, target2);   
    }

    public void CreatePlayerDialogueObject(string text)
    {
        GameObject newPlayerDialogue = Instantiate(playerDialoguePrefab, dialogueBox.transform);
        newPlayerDialogue.GetComponent<PlayerDialogueObject>().SetText(text);   
    }

    public void MoveToNextDialogueObject()
    {
        ResetTextIndex();
        currSpeakerIndex++;
    }

    public void MoveToTargetDialogueObject(int target)
    {
       
        ResetTextIndex();
        currSpeakerIndex = target;
    }
}
