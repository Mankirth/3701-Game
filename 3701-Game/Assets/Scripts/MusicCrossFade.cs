using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MusicCrossFade : MonoBehaviour
{

    private StudioEventEmitter musicEventEmitter;
    public static string ParameterName;


    private void Awake()
    {
        musicEventEmitter = GetComponent<StudioEventEmitter>(); 
    }

    
    public void SetHUBMusic(HUBTracks track)
    {
        musicEventEmitter.SetParameter(ParameterName, (float)track);
    }

  

}

public enum HUBTracks
{
    SWAN_PREP = 0,
    SWAN_DIALOGUE = 1,
    PRINCE_PREP = 2,
    PRINCE_DIALOGUE = 3
}

