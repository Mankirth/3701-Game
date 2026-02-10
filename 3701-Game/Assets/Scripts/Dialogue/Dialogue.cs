using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogue { 
    public string characterName;
    public string spriteArt;
    public string[] text;

 
}

[System.Serializable]
public class DialogueList
{
    public Dialogue[] dialogue; //stores multiple dialogue objects from the Json 

}