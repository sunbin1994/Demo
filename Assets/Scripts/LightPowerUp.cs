using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPowerUp : MonoBehaviour
{
    public AudioSource light_audio;

    void playAudioSource()
    {
        light_audio.Play();
    }
}
