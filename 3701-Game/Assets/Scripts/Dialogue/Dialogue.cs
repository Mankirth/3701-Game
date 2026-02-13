using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[System.Serializable]
public class Dialogue {
    public int currIndex;
    public string characterName;
    public string[] text;
    public bool endDialogue;

    public bool hasDecision;
    public bool isDecisionResult;
    public int[] targetIndex;

    public bool canGainConditional;
    public string conditional;
    public int conditionalIndex;
    public bool needsStoryConditional;



}


[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogue; //stores multiple dialogue objects from the Json 

}