using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryAudio : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip liveBatterySound;
    public AudioClip deadBatterySound;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void PlayBatteryChargeSound()
    {
        audio.PlayOneShot(liveBatterySound);
    }

    public void PlayBatteryDeadSound()
    {
        audio.PlayOneShot(deadBatterySound);
    }




}
