using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioSource sfxAudio;
    [SerializeField] List<AudioClip> sfxList;
    public SoundOptionSave soundSave;
    private float sfxCycle;


    // Start is called before the first frame update
    void Start()
    {
        sfxCycle = (bgmAudio.clip.length / 12);
        StartCoroutine(BackGroundSFX());
        InitOption();
    }

    public void setBGM(float volume)
    {
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void setSFX(float volume)
    {
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }


    public void BGMonoff(bool value, float volume)
    {
        if(value)
        {
            mixer.SetFloat("BGM", Mathf.Log10(0.001f) * 20);
        }
        else
        {
            mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
    }

    public void SFXonoff(bool value, float volume)
    {
        if (value)
        {
            mixer.SetFloat("SFX", Mathf.Log10(0.001f) * 20);
        }
        else
        {
            mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        }
    }

    private IEnumerator BackGroundSFX()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(sfxCycle - 0.2f);

            sfxAudio.clip = sfxList[Random.Range(0, sfxList.Count)];
            sfxAudio.Play();
            yield return new WaitForSecondsRealtime(0.2f);
            sfxAudio.Play();
        }
    }

    private void InitOption()
    {
        BGMonoff(soundSave.bgmMute, soundSave.bgmSound);
        SFXonoff(soundSave.sfxMute, soundSave.sfxSound);
    }
}
