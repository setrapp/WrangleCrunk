using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSound : MonoBehaviour
{
    private AudioSource audio;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audio.Play();
    }

    public void StopSound()
    {
        audio.Stop();
    }
}
