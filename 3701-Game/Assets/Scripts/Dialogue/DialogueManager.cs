using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    public TextAsset dialogueJson;
    DialogueList dialogueData; //You can find this class in Dialogue.cs

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     LoadJsonFile();


    }

    void LoadJsonFile ()
    {
        //get file path from Json file from TextAsset in inspector
        string filePath = dialogueJson.text;

        //apply all Json items into our data container
        dialogueData = JsonUtility.FromJson<DialogueList>(filePath);

      
    }
}
