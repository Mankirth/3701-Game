using UnityEngine;
using UnityEngine.UI;

public class HUBStateManager : MonoBehaviour
{
    public MusicCrossFade musicCrossFade;
    [SerializeField] private NarrativeProgression narrProg;
    //These ENUMs are stored in MusicCrossFade.cs -- generally keeping all of them there for organization
    [SerializeField] private HUBTracks HUBTheme;
    [SerializeField] private HUBTracks DialogueTheme;
    [SerializeField] private Image[] NPCHeads;
   
    
    // This script will be used to load the objects in the HUB as well as music
    void Awake()
    {
        MusicCrossFade.ParameterName = "HUBTransition";
        musicCrossFade = GetComponent<MusicCrossFade>();
        CheckNextRival(); //check current rival state
         //okay not starting with the best practice but the HUB will only use HUBTransition parameter in FMOD
      
    }

    //TODO: have this check game state and load the correct enum pointers for the FMOD parameter
    public void CheckNextRival()
    {
        switch (narrProg.currRivalToFight)
        {
            case NarrativeProgression.FightableRival.Swan:
                HUBTheme = HUBTracks.SWAN_PREP;
           
                break;
            case NarrativeProgression.FightableRival.Prince:
                HUBTheme = HUBTracks.PRINCE_PREP;
                break;
        }
        //access some JSON to check which is the current rival you must fight
    
        LoadNPCHeadsInRooms();
        PlayHubTheme();


    }

   
    public void PlayDialogueTheme(string name)
    {
        switch(name)
        {
            case "Swan":
                DialogueTheme = HUBTracks.SWAN_DIALOGUE;
                break;
            case "Prince":
                DialogueTheme = HUBTracks.PRINCE_DIALOGUE;
                break;
        }
        PlaySong(DialogueTheme);
    }

    public void PlaySong(HUBTracks temp)
    {
        musicCrossFade.SetHUBMusic(temp);
    }

   
    public void PlayHubTheme()
    {
        PlaySong(HUBTheme);
    }

    //Check if NPC is dead, if not, make sprite visible
    private void LoadNPCHeadsInRooms()
    {
        if (narrProg.swanStatus == NarrativeProgression.NPCStatus.Dead) NPCHeads[2].color = new Color(0f, 0f, 0f, 0f);
        if (narrProg.princeStatus == NarrativeProgression.NPCStatus.Dead) NPCHeads[4].color = new Color(0f, 0f, 0f, 0f);

    }
}
