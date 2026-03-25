using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlaytestShortcuts : MonoBehaviour
{
    InputAction toHub, toDrunk, toSwan, toPrince;
    [SerializeField]
    private PlayerSettings playerSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toHub = InputSystem.actions.FindAction("SkipToHub");
        toDrunk = InputSystem.actions.FindAction("SkipToDrunkard");
        toSwan = InputSystem.actions.FindAction("SkipToSwan");
        toPrince = InputSystem.actions.FindAction("SkipToPrince");
        playerSettings.ResetToDefault();
        DontDestroyOnLoad(gameObject);
        if(GameObject.Find("PlaytestShortcuts") != gameObject)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (toHub.WasPressedThisFrame())
            JumpTo("HubNavigationTest");
        else if (toDrunk.WasPressedThisFrame())
            JumpTo("SampleScene");
        else if (toSwan.WasPressedThisFrame())
            JumpTo("Tournament");
        else if (toPrince.WasPressedThisFrame())
            JumpTo("Prince");
    }

    private void JumpTo(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
}
