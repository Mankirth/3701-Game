using System.Data;
using System.IO;
using UnityEngine;

public class NPCRelationshipTracker : MonoBehaviour
{
    public TextAsset pointsJson;


    RelationshipPoints currRP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadJsonFile();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadJsonFile()
    {
        //get file path from Json file from TextAsset in inspector
        string filePath = pointsJson.text;

        //apply all Json items into our data container
        currRP = JsonUtility.FromJson<RelationshipPoints>(filePath);


    }

    public void UpdateRP(string key, int val)
    {

        //update points
        switch (key)
        {
            case "swan":
                currRP.swan = val;
                break;
        }

        string filePath = pointsJson.text;

        string json = JsonUtility.ToJson(currRP, true);

       File.WriteAllText(filePath, json);

        Debug.Log($"Game data {key} : {val} to path: {filePath}");

    }
}
