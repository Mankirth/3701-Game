using System.Data;
using System.IO;
using Unity.Hierarchy;
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

    public bool PlayerMeetsRequirement(string name, int requirement)
    {
        bool flag = false;

        switch (name)
        {
            case "swan":
                flag = currRP.swan >= requirement;
                break;
            case "prince":
                flag = currRP.prince >= requirement;
                break;
            case "zealot":
                flag = currRP.zealot >= requirement;
                break;
            case "patriot":
                flag = currRP.patriot >= requirement;
                break;
            case "fox":
                flag = currRP.fox >= requirement;
                break;
            case "devil":
                flag = currRP.devil >= requirement;
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
            case "prince":
                currRP.prince += val;
                break;
            case "zealot":
                currRP.zealot += val;
                break;
            case "patriot":
                currRP.patriot += val;
                break;
            case "fox":
                currRP.fox += val;
                break;
            case "devil":
                currRP.devil += val;
                break;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "points.json");
        

        string json = JsonUtility.ToJson(currRP, true);



        File.WriteAllText(filePath, json);

        Debug.Log($"Game data {key} : {val} to path: {filePath}");

    }

    public string CheckNotoriety()
    {
        if (currRP.notoriety > 10)
        {
            return "WICKED";
        }
        if (currRP.notoriety < 10 && currRP.notoriety > 5)
        {
            return "BAD";
        }
        if (currRP.notoriety < -10)
        {
            return "HERIOC";
        }
        if (currRP.notoriety > -10 && currRP.notoriety < -5)
        {
            return "GOOD";
        }
        return "neutral";
    }


}
