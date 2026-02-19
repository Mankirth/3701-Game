using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[System.Serializable]
public class Dialogue
{
    public int currIndex;
    public string characterName;
    public string[] text;
 
    public int[] targetIndex; //can possibly be null
    public bool decision;
    public int[] relationshipPoints;
    public int pointRequirement;
    public bool endDialogueEarly;

 
}

[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogue; //stores multiple dialogue objects from the Json 

}