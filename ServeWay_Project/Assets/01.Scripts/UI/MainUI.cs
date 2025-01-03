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

    CloseStart
}

public class MainUI : MonoBehaviour
{
    public GameObject openingButton;

    private void Start()
    {
        if(!GameManager.gameManager.charData.saveFile.isTuto)
        {
            openingButton.SetActive(false);
        }
    }
}
