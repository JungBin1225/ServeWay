using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSound : MonoBehaviour
{
    private List<AudioSource> sounds;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        sounds = new List<AudioSource>();

        for (int i = 0; i < transform.childCount; i++)
        {
            sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
            
            if(i >= 5)
            {
                sounds[i].Play();
            }
            else
            {
                StartCoroutine(PlaySound(sounds[i]));
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        foreach(AudioSource audio in sounds)
        {
            audio.Stop();
        }
    }

    void Update()
    {
        
    }

    IEnumerator PlaySound(AudioSource audio)
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            audio.Play();
        }
    }
}
