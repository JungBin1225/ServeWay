using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionBG;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject optionMenuBtns;

    [SerializeField] private GameObject pauseBG;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuBtns;

    public int BGMVolume = 0;
    public int SFXVolume = 0;

    // Start is called before the first frame update
    void Start()
    {
        BGMVolume = 0;
        SFXVolume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
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
}
