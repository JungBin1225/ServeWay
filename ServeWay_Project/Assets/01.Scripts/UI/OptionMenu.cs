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

    public int BGMVolume = 0;
    public int SFXVolume = 0;

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
    }

    public void setBGMVolume()
    {
        sm.setVolume(BGMSlider.value);
    }

    public void onoffBGM()
    {
        sm.onoff(!BGMToggle.isOn);  // toggle true: mute false / toggle false: mute true
        if (!BGMToggle.isOn)    // ON
        {
            gameObject.transform.GetChild(2).transform.GetChild(1).transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else if (BGMToggle.isOn)    // OFF
        {
            gameObject.transform.GetChild(2).transform.GetChild(1).transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "ON";
        }
    }
}
