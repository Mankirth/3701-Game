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
        string persistentPath = Path.Combine(Application.persistentDataPath, "points.json");
        string jsonText;

        // Load from persistent data if it exists, otherwise use the initial asset
        if (File.Exists(persistentPath))
        {
            jsonText = File.ReadAllText(persistentPath);
        }
        else
        {
            jsonText = pointsJson.text;
        }

        //apply all Json items into our data container by checking tags
        currRP = JsonUtility.FromJson<RelationshipPoints>(jsonText);
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

    public void MarkNPCTalked(string npcName)
    {
        switch (npcName)
        {
            case "swan":
                currRP.swanTalked = true;
                Debug.Log("Swan has been MARKED as talked. "+currRP.swanTalked);
                break;
            case "prince":
                currRP.princeTalked = true;
                Debug.Log("Prince has been MARKED as talked. "+currRP.princeTalked);
                break;
            case "zealot":
                currRP.zealotTalked = true;
                break;
            case "patriot":
                currRP.patriotTalked = true;
                break;
            case "fox":
                currRP.foxTalked = true;
                break;
            case "devil":
                currRP.devilTalked = true;
                break;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "points.json");
        string json = JsonUtility.ToJson(currRP, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"Marked NPC as talked: {npcName} | Path: {filePath}" + currRP.princeTalked);
    }

    public bool AllNPCsTalked()
    {
        return currRP.swanTalked
            && currRP.princeTalked;
            // && currRP.zealotTalked
            // && currRP.patriotTalked
            // && currRP.foxTalked
            // && currRP.devilTalked;
    }

    public bool HasTalkedTo(string npcName)
    {
        switch (npcName)
        {
            case "swan":
                Debug.Log("Swan has been checked and talked to. "+currRP.swanTalked);
                return currRP.swanTalked;
            case "prince":
                Debug.Log("Prince has been checked and talked to. "+currRP.princeTalked);
                return currRP.princeTalked;
            case "zealot":
                return currRP.zealotTalked;
            case "patriot":
                return currRP.patriotTalked;
            case "fox":
                return currRP.foxTalked;
            case "devil":
                return currRP.devilTalked;
            default:
                return false;
        }
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
