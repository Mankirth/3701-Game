using FMOD.Studio;
using FMODUnity;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MusicCrossFade : MonoBehaviour
{

  private StudioEventEmitter musicEventEmitter;
    public static string ParameterName;


    
    
    public void SetHUBMusic(HUBTracks track)
    {
        if (musicEventEmitter == null) musicEventEmitter = GetComponent<StudioEventEmitter>(); // set up for the first time
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

