using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public ArtConfiguration dialogueCanvas;
    public TextAsset dialogueJson;

    public NPCRelationshipTracker relationshipTracker;

    public GameObject dialogueBox;
    public GameObject dialoguePrefab;
    public GameObject decisionPrefab;
    public GameObject playerDialoguePrefab;
    public GameObject exitPrefab;

    public Scrollbar scrollBar;
    
    DialogueList dialogueData; //You can find this class in Dialogue.cs

    Dialogue currDialogue;
    public enum SpeakerState { Speaking, Decision, Finish};
    public SpeakerState speakerState;

    public static event System.Action<string> OnDialogueCompleted;

    bool dialogueReadyToExit;
    bool stopRendering;

    public enum DecisionState { NotCreated, Waiting };
    public DecisionState decisionState;

    public int currSpeakerIndex;
    public int currTextIndex;

    string NPCName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetSpeakerIndex();
        ResetTextIndex();
        LoadJsonFile();
        decisionState = DecisionState.NotCreated; //start off as waiting because no dialogue option has been chosen
        dialogueReadyToExit = false;
        stopRendering = false;
      
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogueCanvas.isOnScreen && !dialogueReadyToExit){
            HandleInput();
        } else if (!stopRendering && dialogueReadyToExit) //we are ready to exit, create exit object and then stop rendering
        {
            CreateExitObject();
            stopRendering = true;

        }

    }
    public void LoadJsonFile()
    {
        //get pure text data from json
        string filePath = dialogueJson.text;

        //apply all Json items into our data container
        dialogueData = JsonUtility.FromJson<DialogueList>(filePath);

        //Find who the speaker is and store a reference to that name
        int i = 0;
        while (NPCName == "")
        {
            if (dialogueData.dialogue[i].characterName != "fencer")
            {
                NPCName = dialogueData.dialogue[i].characterName;
            }
            i++;
        }
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

                        CreateDecisionObject(currDialogue.text[0],
                            currDialogue.text[1],
                                currDialogue.targetIndex[0],
                                    currDialogue.targetIndex[1]);
                    } 
              
                    break;
            case SpeakerState.Finish:
                RenderDialogue();  //TODO: NEED TO FIX IT SO YOU CAN PRESS ANOTHER KEY TO EXIT
               
                dialogueReadyToExit = true;

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

       
        string text = currDialogue.text[currTextIndex];
        string speaker = currDialogue.characterName;

        if (speaker == NPCName)
        {
            
            CreateDialogueObject(text);
        } else
        {
            CreatePlayerDialogueObject(text);
          
        }



    }

    public void CheckSpeakerState()
    {
        currDialogue = dialogueData.dialogue[currSpeakerIndex]; //create reference to current Dialogue Object -> makes stuff readable

        if (!IsThereDialogue() || currDialogue.endDialogueEarly) speakerState = SpeakerState.Finish;       //we have no more dialogue objects to get through
        else if (currDialogue.decision) speakerState = SpeakerState.Decision; //we are waiting on a decision
        else speakerState = SpeakerState.Speaking; //We have regular lines to render

    }
    public void RenderDialogueHandler()
    {
        if (currTextIndex < 5)
        {
            Debug.Log("MOTOWADAD: " + relationshipTracker.CheckNotoriety());
            switch (relationshipTracker.CheckNotoriety())
            {
                case "WICKED":
                    currDialogue.currIndex = 1;
                    break;
                case "BAD":
                    currDialogue.currIndex = 2;
                    break;
                case "GOOD":
                    currDialogue.currIndex = 3;
                    break;
                case "HEROIC":
                    currDialogue.currIndex = 4;
                    break;
                default:
                    currDialogue.currIndex = 0;
                    break;
            }

        }

        //Check if the dialogue requires points
        if (currDialogue.pointRequirement > 0)
        {
            Debug.Log("OP1: We have something to render that requires points");
            if (!relationshipTracker.PlayerMeetsRequirement(
                    NPCName, 
                        currDialogue.pointRequirement))
            {
                Debug.Log("OP1: Relationship points not met, playing latter response");
                currTextIndex = 1; //relationship points not met, play the latter response
            }

            Debug.Log("OP1: Playing determinent response");
            RenderDialogue(); 

           if (currDialogue.targetIndex[currTextIndex] > 0)
            {
                Debug.Log("OP1: We have somewhere to go");
                MoveToTargetDialogueObject(currDialogue.targetIndex[currTextIndex]);
                HandleInput();  //run it back
            } else
            {
                Debug.Log("OP1: We don't have somewhere to go, move on to the next object");
                MoveToNextDialogueObject(); //move on to next potential dialogue object
                HandleInput();  //run it back
            }

            
  
        }
            //check if we have lines to render
            else if (IsThereText())
            {
            Debug.Log("OP2: Render normal dialogue.");
            RenderDialogue();    //render current line
            
               
              currTextIndex++; //increment text line
            }
        //check if we are on the correct target dialogue (i.e: just coming out of a player choice and this is post-response)
        else if    
                (currDialogue.targetIndex[0] > 0) //we will never return to the beginning, so use this as benchmark
            {
            Debug.Log("OP3: We've run out of text objects, need to move to target");
            MoveToTargetDialogueObject(currDialogue.targetIndex[0]); //move to target line and exit choice tree
                HandleInput(); //run it back
        }
        else
        {
            Debug.Log("OP4: We've run out of text objects, move onto next normally.");
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
        return currTextIndex < currDialogue.text.Length;
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
        Invoke("RenderScrollBarDown", 0.025f);
    }

    public void CreateExitObject()
    {
        GameObject newDecision = Instantiate(exitPrefab, dialogueBox.transform);
        newDecision.GetComponent<DialogueExitObject>().SetUp(dialogueCanvas);
        Invoke("RenderScrollBarDown", 0.025f);
    }
    public void CreateDecisionObject(string text1, string text2, int target1, int target2)
    {
        decisionState = DecisionState.Waiting; //created, waiting for player response

        GameObject newDecision = Instantiate(decisionPrefab, dialogueBox.transform);
        newDecision.GetComponent<PlayerChoiceObject>().SetText(text1, text2, target1, target2);
        Invoke("RenderScrollBarDown", 0.025f);
    }

    public void CreatePlayerDialogueObject(string text)
    {
        GameObject newPlayerDialogue = Instantiate(playerDialoguePrefab, dialogueBox.transform);
        newPlayerDialogue.GetComponent<PlayerDialogueObject>().SetText(text);
        Invoke("RenderScrollBarDown", 0.025f);
    }

    public void MoveToNextDialogueObject()
    {
        ResetTextIndex();
        currSpeakerIndex++;
    }

    //Move to next branch of tree, considered a reset of dialogue from the previous passage
    public void MoveToTargetDialogueObject(int target)
    {
       
        ResetTextIndex();
        currSpeakerIndex = target;
    }

    public void AddPoints(int value)
    {
        //Accesses relationship tracker's script to update the relationship points earned and write to the JSON
        //This is tied strictly to the player option objectsl only their script calls this function
        relationshipTracker.UpdateRP(NPCName, currDialogue.relationshipPoints[value]);
    }

    public void RenderScrollBarDown()
    {
        scrollBar.value = 0;
    }
}
