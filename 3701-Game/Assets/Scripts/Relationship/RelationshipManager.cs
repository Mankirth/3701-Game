using UnityEditor;
using UnityEngine;
using System.IO;

/* Shared instance with relationship stats
 * Contains user notoriety, a list of which opponents were killed & which opponents were spared
 * Used in all scenes, do not need to apply to gameobject. Just reference the script
 * Increase/decrease notoriety at the end of each level based on user decision
 */

[CreateAssetMenu(fileName = "RelationshipManager", menuName = "ScriptableObjects/RelationshipManager")]
public class RelationshipManager : ScriptableObject
{
    public TextAsset pointsJson;
    public RelationshipPoints currRP;



    [SerializeField]
    private int maxNotoriety = 15;

    public void Awake()
    {
        LoadJsonFile();
       
    }

    public void IncreaseNotoriety(int notorietyVal, int RPVal)
    {
        currRP.notoriety += notorietyVal;

        currRP.swan -= RPVal;
        currRP.prince -= RPVal;
        currRP.patriot -= RPVal;
        currRP.zealot -= RPVal;
        currRP.devil += RPVal;


        string filePath = AssetDatabase.GetAssetPath(pointsJson);
        string json = JsonUtility.ToJson(currRP, true);
        Debug.Log(json);

        File.WriteAllText(filePath, json);
    }
    
    public void DecreaseNotoriety(int notorietyVal, int RPVal)
    {
        currRP.notoriety -= notorietyVal;

        currRP.swan += RPVal;
        currRP.prince += RPVal;
        currRP.patriot += RPVal;
        currRP.zealot += RPVal;
        currRP.devil -= RPVal;


        string filePath = AssetDatabase.GetAssetPath(pointsJson);
        string json = JsonUtility.ToJson(currRP, true);
        Debug.Log(json);

        File.WriteAllText(filePath, json);
    }

    public void LoadJsonFile()
    {
        //get pure text data from json
        string filePath = pointsJson.text;
        currRP = JsonUtility.FromJson<RelationshipPoints>(filePath);


    }

}
