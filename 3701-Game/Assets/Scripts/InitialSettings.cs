using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialSettings : MonoBehaviour
{
    public PlayerSettings settings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settings.SetDifficultyPreset(PlayerSettings.Difficulty.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToHUB()
    {
        SceneManager.LoadScene("HubNavigationTest");
    }

}
