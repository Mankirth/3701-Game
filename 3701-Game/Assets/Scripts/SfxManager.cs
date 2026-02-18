using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioClip metronome, windUp, playerDodge, parry, enemyHit;
    [SerializeField]
    private AudioSource Onbeat, Offbeat;
    
    public void PlayOnBeat(AudioClip clip)
    {
        Onbeat.Stop();
        Onbeat.clip = clip;
        Onbeat.Play();
    }

    public void PlayOffBeat(AudioClip clip)
    {
        
        Offbeat.clip = clip;
        Offbeat.Play();
    }
}
