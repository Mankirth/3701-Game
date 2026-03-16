using UnityEngine;

public class DialogueExitObject : MonoBehaviour
{
    ArtConfiguration config;

    public void SetUp(ArtConfiguration temp)
    {
        config = temp; //store art config reference
    }
    public void ExitDialogue()
    {
        config.OffLoadScreen();
        Debug.Log("Exiting dialogue");
    }
   
}
