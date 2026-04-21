using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class HUDTest
{
    private static readonly string[] UIObjects =
   {
        "HUD Canvas",
        "HUD",
        "INTRO MENU",
        "PHASE MENU",
        "PauseMenu"
    };
    
    [Test]
    public void HUDEnabled_OnSceneLoad_HUDIsActive()
    {
        string[] scenes = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes/Gameplay" });

        foreach (string guid in scenes)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(path);

            foreach (string ui in UIObjects)
            {
                GameObject obj = GameObject.Find(ui);

                Assert.IsNotNull(obj, $"[CI FAIL] Missing UI object '{ui}' in scene: {path}"); // Assert means it fails if it is null in this case

                Assert.IsTrue(obj.activeInHierarchy, $"[CI FAIL] UI object '{ui}' is DISABLED in scene: {path}"); // Assert means fails if it is false in this case

            }
        }

        Debug.Log("Gameplay UI validation successful.");
    }
}
