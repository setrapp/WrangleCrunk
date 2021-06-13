using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAudio : MonoBehaviour
{

    public AudioClip[] excitedClips;
    public AudioClip[] disgruntledClips;

    private AudioSource audio;
    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void PlayExcitedClip()
    {
        AudioClip clip = excitedClips[Random.Range(0, excitedClips.Length - 1)];
        audio.PlayOneShot(clip);
    }

    public void PlayDisgruntledClip()
    {
        AudioClip clip = disgruntledClips[Random.Range(0, excitedClips.Length - 1)];
        audio.PlayOneShot(clip);
    }
}
