using UnityEngine;

public class FMODAudioBusController : MonoBehaviour
{
    FMOD.Studio.Bus master;

    //private void Awake()
    //{
    //    master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    //}

    public void UpdateMasterVolume(float volume)
    {
        master.setVolume(volume);
    }
}
