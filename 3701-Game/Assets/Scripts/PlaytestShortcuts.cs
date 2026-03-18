using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlaytestShortcuts : MonoBehaviour
{
    InputAction toHub, toDrunk, toSwan, toPrince;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toHub = InputSystem.actions.FindAction("SkipToHub");
        toDrunk = InputSystem.actions.FindAction("SkipToDrunkard");
        toSwan = InputSystem.actions.FindAction("SkipToSwan");
        toPrince = InputSystem.actions.FindAction("SkipToPrince");
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (toHub.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("HubNavigationTest");
        }
        else if (toDrunk.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("SampleScene");
        }
        else if (toSwan.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Tournament");
        }
        else if (toPrince.WasPressedThisFrame())
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Prince");
        }
    }
}
