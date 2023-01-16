using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterVisuals : MonoBehaviour
{
    public ParticleSystem snowParticles;
    private void Update()
    {
        if (TimeManager.Instance.Winter())
        {
            if (!snowParticles.isPlaying)
            {
                snowParticles.Play();
            }
        }
        else
        {
            if (snowParticles.isPlaying)
            {
                snowParticles.Stop();
            }
        }
    }
}
