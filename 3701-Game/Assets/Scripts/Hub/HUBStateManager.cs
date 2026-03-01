using UnityEngine;

public class HUBStateManager : MonoBehaviour
{
    public MusicCrossFade musicCrossFade;
    //These ENUMs are stored in MusicCrossFade.cs -- generally keeping all of them there for organization
    [SerializeField] private HUBTracks NPCTheme;
    [SerializeField] private HUBTracks NPCDialogue;

    
    // This script will be used to load the objects in the HUB as well as music
    void Start()
    {
      
        musicCrossFade = GetComponent<MusicCrossFade>();
        CheckNextRival(); //check current rival state
        MusicCrossFade.ParameterName = "HUBTransition";     //okay not starting with the best practice but the HUB will only use HUBTransition parameter in FMOD
      
    }

    //TODO: have this check game state and load the correct enum pointers for the FMOD parameter
    public void CheckNextRival()
    {
        //access some JSON to check which is the current rival you must fight
        NPCTheme = HUBTracks.SWAN_PREP;
        NPCDialogue = HUBTracks.SWAN_DIALOGUE;
    }

    public void PlayHUBTheme()
    {
        musicCrossFade.SetHUBMusic(NPCTheme);
    }

    public void PlayDialogueTheme()
    {
        musicCrossFade.SetHUBMusic(NPCDialogue);
    }
}
