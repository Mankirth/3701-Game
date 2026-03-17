using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioClip metronome, windUp, playerDodge, parry, enemyHit;
    [SerializeField]
    private AudioSource Onbeat, Offbeat, sfx;
    
    public void QueueSound(bool onBeat, AudioClip clip, float pitch)
    {
        PlaySound(onBeat ? Onbeat : Offbeat, clip, pitch);
    }

    public void QueueSound(bool onBeat, AudioClip clip)
    {
        PlaySound(onBeat ? Onbeat : Offbeat, clip, 1);
    }

    private void PlaySound(AudioSource source, AudioClip clip, float pitch)
    {
        source.Stop();
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }

    public void SetVolume(float volume)
    {
        Onbeat.volume = volume;
        Offbeat.volume = volume;
        sfx.volume = volume;
    }
}
