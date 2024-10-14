using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int BGMVolume = 0;
    public int SFXVolume = 0;

    public Sprite volumeOn;
    public Sprite volumeOff;

    // Start is called before the first frame update
    void Start()
    {
        BGMVolume = 0;
        SFXVolume = 0;

        sm = sm.GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (optionBG.activeSelf || optionPanel.activeSelf || optionMenuBtns.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoBack();
            }
        }
    }

    public void GoBack()
    {
        optionMenuBtns.SetActive(false);
        optionPanel.SetActive(false);
        optionBG.SetActive(false);

        pauseBG.SetActive(true);
        pausePanel.SetActive(true);
        pauseMenuBtns.SetActive(true);

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
        menuClick.Play();
        sm.BGMonoff(!BGMToggle.isOn, BGMSlider.value);  // toggle true: mute false / toggle false: mute true
        if (!BGMToggle.isOn)    // ON
        {
            gameObject.transform.GetChild(2).transform.GetChild(1).transform.GetChild(4).transform.GetChild(0).GetComponent<Image>().sprite = volumeOff;
        }
        else if (BGMToggle.isOn)    // OFF
        {
            gameObject.transform.GetChild(2).transform.GetChild(1).transform.GetChild(4).transform.GetChild(0).GetComponent<Image>().sprite = volumeOn;
        }
    }

    public void onoffFX()
    {
        menuClick.Play();
        sm.SFXonoff(!SFXToggle.isOn, SFXSlider.value);
        if (!SFXToggle.isOn)    // ON
        {
            gameObject.transform.GetChild(2).transform.GetChild(2).transform.GetChild(4).transform.GetChild(0).GetComponent<Image>().sprite = volumeOff;
        }
        else if (SFXToggle.isOn)    // OFF
        {
            gameObject.transform.GetChild(2).transform.GetChild(2).transform.GetChild(4).transform.GetChild(0).GetComponent<Image>().sprite = volumeOn;
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
}
