using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionBG;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject optionMenuBtns;

    [SerializeField] private GameObject pauseBG;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuBtns;

    [SerializeField] private SoundManager sm;

    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Toggle BGMToggle;

    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Toggle SFXToggle;

    [SerializeField] private AudioSource menuOpen;
    [SerializeField] private AudioSource menuClick;

    public Sprite volumeOn;
    public Sprite volumeOff;

    // Start is called before the first frame update
    void Start()
    {
        sm = sm.GetComponent<SoundManager>();

        InitValue();
            
    }

    // Update is called once per frame
    void Update()
    {
        if(!SceneManager.GetActiveScene().name.Contains("Title"))
        {
            if (optionBG.activeSelf || optionPanel.activeSelf || optionMenuBtns.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GoBack();
                }
            }
        }
    }

    public void GoBack()
    {
        if (pauseBG != null)
        {
            optionMenuBtns.SetActive(false);
            optionPanel.SetActive(false);
            optionBG.SetActive(false);

            pauseBG.SetActive(true);
            pausePanel.SetActive(true);
            pauseMenuBtns.SetActive(true);
        }

        PlayerPrefs.SetString("BGM_Mute", (!BGMToggle.isOn).ToString());
        PlayerPrefs.SetFloat("BGM_Sound", BGMSlider.value);

        PlayerPrefs.SetString("SFX_Mute", (!SFXToggle.isOn).ToString());
        PlayerPrefs.SetFloat("SFX_Sound", SFXSlider.value);

        PlayerPrefs.Save();

        menuOpen.Play();
    }

    public void setBGMVolume()
    {
        sm.setBGM(BGMSlider.value);
        if(!BGMToggle.isOn)
        {
            BGMToggle.isOn = true;
        }
    }

    public void setSFXVolume()
    {
        sm.setSFX(SFXSlider.value);
        if (!SFXToggle.isOn)
        {
            SFXToggle.isOn = true;
        }
    }

    public void onoffBGM()
    {
        if(optionMenuBtns.activeSelf && (GetComponent<CanvasGroup>() == null || GetComponent<CanvasGroup>().alpha != 0))
        {
            menuClick.Play();
        }

        sm.BGMonoff(!BGMToggle.isOn, BGMSlider.value);  // toggle true: mute false / toggle false: mute true
        if (!BGMToggle.isOn)    // ON
        {
            BGMToggle.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = volumeOff;
        }
        else if (BGMToggle.isOn)    // OFF
        {
            BGMToggle.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = volumeOn;
        }
    }

    public void onoffFX()
    {
        if (optionMenuBtns.activeSelf && (GetComponent<CanvasGroup>() == null || GetComponent<CanvasGroup>().alpha != 0))
        {
            menuClick.Play();
        }
            
        sm.SFXonoff(!SFXToggle.isOn, SFXSlider.value);
        if (!SFXToggle.isOn)    // ON
        {
            SFXToggle.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = volumeOff;
        }
        else if (SFXToggle.isOn)    // OFF
        {
            SFXToggle.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = volumeOn;
        }
    }

    public void OnBackPressed(RectTransform text)
    {
        text.offsetMin -= new Vector2(0, 10);
        text.offsetMax -= new Vector2(0, 10);
        menuClick.Play();
    }

    public void OnBackRelease(RectTransform text)
    {
        text.offsetMin += new Vector2(0, 10);
        text.offsetMax += new Vector2(0, 10);
        menuClick.Play();
    }

    public void InitValue()
    {
        if(PlayerPrefs.HasKey("BGM_Sound"))
        {
            bool bgmTemp = bool.Parse(PlayerPrefs.GetString("BGM_Mute"));
            bool sfxTemp = bool.Parse(PlayerPrefs.GetString("SFX_Mute"));

            BGMSlider.value = PlayerPrefs.GetFloat("BGM_Sound");
            SFXSlider.value = PlayerPrefs.GetFloat("SFX_Sound");
            BGMToggle.isOn = !bgmTemp;
            SFXToggle.isOn = !sfxTemp;
        }
        else
        {
            BGMSlider.value = 1;
            SFXSlider.value = 1;
            BGMToggle.isOn = true;
            SFXToggle.isOn = true;
        }
    }
}
