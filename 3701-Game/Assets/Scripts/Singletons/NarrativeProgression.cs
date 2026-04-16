using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrativeProgression", menuName = "Scriptable Objects/NarrativeProgression")]
public class NarrativeProgression : ScriptableObject
{
    public FightableRival currRivalToFight = FightableRival.Swan;

    public NPCDialogueState swanDialogueState = NPCDialogueState.PreFight;
    public NPCStatus swanStatus = NPCStatus.Alive;


    public NPCDialogueState princeDialogueState = NPCDialogueState.PreFight;
    public NPCStatus princeStatus = NPCStatus.Alive;

  
    public void ResetGame()
    {
        princeStatus = NPCStatus.Alive;
        swanStatus = NPCStatus.Alive;

        swanDialogueState = NPCDialogueState.PreFight;
        princeDialogueState = NPCDialogueState.PreFight;

        currRivalToFight = FightableRival.Swan;
    }

    
    [Serializable]
    public enum FightableRival
    {
        Swan,
        Prince
    }

    public enum NPCDialogueState
    {
        PreFight,
        PostFight
    }

    public enum NPCStatus
    {
        Dead, 
        Alive
    }

    
    

}
