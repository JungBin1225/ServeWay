using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTNType
{
    Opening,
    Start,
    Option,
    Sound,
    OptionBack,
    New,
    Continue,
    Quit,
    CloseStart,
    Ending
}

public class MainUI : MonoBehaviour
{
    public GameObject openingButton;
    public GameObject endingButton;

    private void Start()
    {
        if(!GameManager.gameManager.charData.saveFile.isTuto)
        {
            openingButton.SetActive(false);
        }

        if(!GameManager.gameManager.charData.saveFile.isEnding)
        {
            endingButton.SetActive(false);
        }
    }
}
