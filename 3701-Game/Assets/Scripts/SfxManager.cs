using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioClip metronome, windUp, playerDodge, parry, enemyHit;
    [SerializeField]
    private AudioSource Onbeat, Offbeat;
    
    public void QueueSound(bool onBeat, AudioClip clip)
    {
        PlaySound(onBeat ? Onbeat : Offbeat, clip);
    }

    private void PlaySound(AudioSource source, AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}
