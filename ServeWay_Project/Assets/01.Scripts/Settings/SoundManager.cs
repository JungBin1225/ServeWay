using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void setVolume(float value)
    {
        audioSource.volume = value;
    }
    public void onoff(bool value)
    {
        audioSource.mute = value;
    }
}
