using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class NPCRelationshipTracker : MonoBehaviour
{
    public TextAsset pointsJson;


    public RelationshipPoints currRP;
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
        //get pure text data from json
        string filePath = pointsJson.text;

        //apply all Json items into our data container by checking tags
        currRP = JsonUtility.FromJson<RelationshipPoints>(filePath);


    }

    public bool PlayerMeetsRequirement(string name, int requiment)
    {
        bool flag = false;

        switch (name)
        {
            case "swan":
                flag = currRP.swan >= requiment; 
                break;
        }
        return flag;
    }
    public void UpdateRP(string key, int val)
    {

        //update points
        switch (key)
        {
            case "swan":
                currRP.swan += val;
                break;
        }

        string filePath = AssetDatabase.GetAssetPath(pointsJson);
;

        string json = JsonUtility.ToJson(currRP, true);

       

       File.WriteAllText(filePath, json);

        Debug.Log($"Game data {key} : {val} to path: {filePath}");

    }
}
