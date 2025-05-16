using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioSource sfxAudio;
    [SerializeField] List<AudioClip> sfxList;
    [SerializeField] AudioClip bossBgm;
    [SerializeField] AudioClip mainBgm;
    private float sfxCycle;


    // Start is called before the first frame update
    void Start()
    {
        sfxCycle = (bgmAudio.clip.length / 12);

        InitOption();
        StartCoroutine(InitBGM());
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

    public IEnumerator SetBossBGM()
    {
        StopCoroutine(BackGroundSFX());
        yield return new WaitForSecondsRealtime(0.1f);

        bgmAudio.clip = bossBgm;
        bgmAudio.Play();
        sfxCycle = (bossBgm.length / 12);

        StartCoroutine(BackGroundSFX());
    }

    public IEnumerator SetMainBGM()
    {
        StopCoroutine(BackGroundSFX());
        yield return new WaitForSecondsRealtime(0.1f);

        bgmAudio.clip = mainBgm;
        bgmAudio.Play();
        sfxCycle = (mainBgm.length / 12);

        StartCoroutine(BackGroundSFX());
    }

    private IEnumerator InitBGM()
    {
        yield return null;
        yield return null;

        bgmAudio.Play();
        if (SceneManager.GetActiveScene().name.Contains("Main"))
        {
            StartCoroutine(BackGroundSFX());
        }
    }

    private void InitOption()
    {
        bool bgmMute = false;
        bool sfxMute = false;
        float bgmValue = 1;
        float sfxValue = 1;

        if (PlayerPrefs.HasKey("BGM_Sound"))
        {
            bgmMute = bool.Parse(PlayerPrefs.GetString("BGM_Mute"));
            sfxMute = bool.Parse(PlayerPrefs.GetString("SFX_Mute"));

            bgmValue = PlayerPrefs.GetFloat("BGM_Sound");
            sfxValue = PlayerPrefs.GetFloat("SFX_Sound");
        }

        BGMonoff(bgmMute, bgmValue);
        SFXonoff(sfxMute, sfxValue);
    }
}
