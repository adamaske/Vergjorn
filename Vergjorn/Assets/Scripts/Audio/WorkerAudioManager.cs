using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAudioManager : MonoBehaviour
{
    public static WorkerAudioManager Instance;

    public List<AudioClip> woodChoppingSounds = new List<AudioClip>();
    public List<AudioClip> myrmalmGatheringSounds = new List<AudioClip>();
    public List<AudioClip> walkingSounds = new List<AudioClip>();
    public List<AudioClip> randomVoiceLines = new List<AudioClip>();
    public List<AudioClip> assignedJobSounds = new List<AudioClip>();
    public List<AudioClip> levelUpSounds = new List<AudioClip>();
    public List<AudioClip> buildingSounds = new List<AudioClip>();


    public AudioSource source;
    private void Awake()
    {
        Instance = this;
    }

    void PlayClip(AudioClip clip)
    {

    }
    
   public void PlayAssignedJobVoiceLine(bool atPos)
    {
        if(assignedJobSounds.Count == 0)
        {
            Debug.Log("Put in assigned jobs voice lines");
            return;
        }

        PlayClip(assignedJobSounds[Random.Range(0, assignedJobSounds.Count)]);
    }
    public void PlayLeveledUpSound(bool atPos)
    {
        if(levelUpSounds.Count == 0)
        {
            Debug.Log("Put in level up sounds");
            return;
        }

        PlayClip(levelUpSounds[Random.Range(0, levelUpSounds.Count)]);
    }

    public void PlayBuildingSounds(bool atPos, Vector3 pos)
    {
        if(buildingSounds.Count == 0)
        {
            return;
        }

        if (atPos)
        {
            AudioSource.PlayClipAtPoint(buildingSounds[Random.Range(0, buildingSounds.Count)], pos);
        }
        else
        {
            //PlayClip()
        }

    }
}
