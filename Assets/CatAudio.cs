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
        int index = Random.Range(0, excitedClips.Length);
        AudioClip clip = excitedClips[index];
        audio.PlayOneShot(clip);
    }

    public void PlayDisgruntledClip()
    {
        int index = Random.Range(0, excitedClips.Length);
        AudioClip clip = disgruntledClips[index];
        audio.PlayOneShot(clip);
    }
}
