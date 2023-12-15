using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleDriveParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    public void ToggleEffect(bool active)
    {
        if (active == true)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }
    
}
