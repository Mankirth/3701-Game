using UnityEngine;

public class RoomSetUp : MonoBehaviour
{
    //This script sets up the room prefab to construct the correct rooms and load in the right dialogue
    [SerializeField] private NarrativeProgression narrProg;
    [SerializeField] private GameObject NPC;
    [SerializeField] private DialogueManager dialogueManager;

  
    public CharacterInRoom currChar;
    public enum CharacterInRoom
    {
        SWAN,
        PRINCE
    }
    
    private void Awake()
    {
        if (!IsCharacterAlive()) //if character is dead, remove them from the scene
        {
            NPC.SetActive(false);
        } else
        {
            LoadDialogueBasedOnState();
        }
    }

    private bool IsCharacterAlive()
    {
        switch (currChar)
        {
            case CharacterInRoom.SWAN:
                if (narrProg.swanStatus == NarrativeProgression.NPCStatus.Alive) return true;
                break;
            case CharacterInRoom.PRINCE:
                if (narrProg.princeStatus == NarrativeProgression.NPCStatus.Alive) return true;
                break;
        }

        return false;
    }

    private void LoadDialogueBasedOnState()
    {
        //chest if pre or post battle first, then load it, otherwise load post-fight
        switch (currChar)
        {
            case CharacterInRoom.SWAN:
                LoadSwanInteraction();
                    break;
            case CharacterInRoom.PRINCE:
                LoadPrinceInteraction();
                break;
        }
    }
    
    private void LoadPrinceInteraction()
    {
        if (narrProg.currRivalToFight == NarrativeProgression.FightableRival.Prince)
        {
            dialogueManager.LoadPreFightDialogue(); //prince talks to you if he's up next
        }
        else
        {
            dialogueManager.LoadDeadEndDialogue(); //otherwise he is not interested
        }
    }

    private void LoadSwanInteraction()
    {
        if (narrProg.swanDialogueState == NarrativeProgression.NPCDialogueState.PreFight)
        {
            dialogueManager.LoadPreFightDialogue();
        }
        else
        {
            dialogueManager.LoadDeadEndDialogue(); //swift conclusion to her arc if she is alive
        }
    }
}
