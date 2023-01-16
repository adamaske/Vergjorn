using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource source;

    public AudioClip[] workerVoiceLines;

    public AudioClip[] babyMakingVoiceLines;

    public AudioClip[] gotTaskVoiceLines;

    public AudioClip[] happyVoiceLines;

    public AudioClip[] sadVoiceLines;

    public AudioClip[] woodCuttingSounds;

    public AudioClip[] gotDestinationSounds;
    public float playAtPosistionMultiplier;
    private void Awake()
    {
        Instance = this;
    }
    public void PlayAtPoint(Vector3 pos, AudioClip clip, float volume)
    {
        float k = 10 * playAtPosistionMultiplier;
        AudioSource.PlayClipAtPoint(clip, pos, k);
    }


    public void PlayWorkerVoiceline(bool atPosistion, float volume, Vector3 pos, AudioType t)
    {
        AudioClip clip = workerVoiceLines[Random.Range(0, workerVoiceLines.Length)];

        if(t == AudioType.gotTask)
        {
            clip = gotTaskVoiceLines[Random.Range(0, gotTaskVoiceLines.Length)];
        }else if(t == AudioType.happy)
        {
            clip = happyVoiceLines[Random.Range(0, happyVoiceLines.Length)];
        }

        if (atPosistion)
        {
            source.PlayOneShot(clip);
            //PlayAtPoint(pos, clip, volume);
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }
    public void PlayWorkerWentToIdle(bool atPosistion, float volume, Vector3 pos)
    {
        AudioClip clip = workerVoiceLines[Random.Range(0, workerVoiceLines.Length)];

       

        if (atPosistion)
        {
            
            PlayAtPoint(pos, clip, volume);
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayWoodChopping(bool atPosistion, float volume, Vector3 pos)
    {
        AudioClip clip = woodCuttingSounds[Random.Range(0, woodCuttingSounds.Length)];

        if (atPosistion)
        {
            PlayAtPoint(pos, clip, volume);
        }
        else
        {
            source.PlayOneShot(clip, volume);
        }
    }

}

public enum AudioType
{
    happy, sad, gotTask, babyMaking
}
