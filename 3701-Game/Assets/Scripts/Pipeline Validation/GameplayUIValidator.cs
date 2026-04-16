using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameplayUIValidator
{
    private static readonly string[] UIObjects =
    {
        "HUD Canvas",
        "HUD",
        "INTRO MENU",
        "PHASE MENU",
        "PauseMenu"
    };

    [MenuItem("CI/Validate Gameplay UI")]
    public static void Validate()
    {
        string[] scenes = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes/Gameplay" });

        foreach (string guid in scenes)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(path);

            foreach (string ui in UIObjects)
            {
                GameObject obj = GameObject.Find(ui);

                if (obj == null)
                {
                    Debug.LogError($"[CI FAIL] Missing UI object '{ui}' in scene: {path}");
                    continue;
                }

                if (!obj.activeInHierarchy)
                {
                    Debug.LogError($"[CI FAIL] UI object '{ui}' is DISABLED in scene: {path}");
                }
            }
        }

        Debug.Log("Gameplay UI validation successful.");
    }
}