using UnityEngine;
using UnityEngine.UI;

public class HUBStateManager : MonoBehaviour
{
    public MusicCrossFade musicCrossFade;
    //These ENUMs are stored in MusicCrossFade.cs -- generally keeping all of them there for organization
    [SerializeField] private HUBTracks NPCTheme;
    [SerializeField] private HUBTracks NPCDialogue;
    [SerializeField] private Image[] NPCHeads;
   
    
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
        LoadNPCHeadsInRooms(); //currently has NO functionality
    }

    public void PlayHUBTheme()
    {
        musicCrossFade.SetHUBMusic(NPCTheme);
    }

    public void PlayDialogueTheme()
    {
        musicCrossFade.SetHUBMusic(NPCDialogue);
    }

    //TODO: relocate sprite heads to assigned rooms
    private void LoadNPCHeadsInRooms()
    {
        //CHECK WHAT NPC SHOULD BE IN WHAT ROOM
        //ASSIGN THEM TO THAT ROOM BY CHANGING THE SRC IMAGE OF SPRITE OBJECT
        //ADJUST OUTLINE SHADER TO MATCH THE TEXTURE
        foreach (Image img in NPCHeads)
        {
           
        }

    }
}
