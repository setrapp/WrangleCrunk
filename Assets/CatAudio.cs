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
        Debug.Log("Playing excited clip");
        AudioClip clip = excitedClips[Random.Range(0, excitedClips.Length - 1)];
        audio.PlayOneShot(clip);
    }

    public void PlayDisgruntledClip()
    {
        Debug.Log("Playing disgruntled clip");
        AudioClip clip = disgruntledClips[Random.Range(0, excitedClips.Length - 1)];
        audio.PlayOneShot(clip);
    }
}
